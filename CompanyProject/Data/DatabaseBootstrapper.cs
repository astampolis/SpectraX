using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Data
{
    public static class DatabaseBootstrapper
    {
        public static async Task EnsureAttendanceAndRulesSchemaAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var sql = @"
IF OBJECT_ID(N'[SystemRules]', N'U') IS NULL
BEGIN
    CREATE TABLE [SystemRules](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(150) NOT NULL,
        [Code] NVARCHAR(100) NOT NULL,
        [IsEnabled] BIT NOT NULL,
        [ValueJson] NVARCHAR(MAX) NULL,
        [UpdatedAt] DATETIME2 NOT NULL,
        [UpdatedBy] NVARCHAR(450) NULL,
        [RowVersion] ROWVERSION NOT NULL
    );
    CREATE UNIQUE INDEX [IX_SystemRules_Code] ON [SystemRules]([Code]);
END;

IF OBJECT_ID(N'[Shifts]', N'U') IS NULL
BEGIN
    CREATE TABLE [Shifts](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [EmployeeId] NVARCHAR(450) NOT NULL,
        [Department] NVARCHAR(100) NULL,
        [StartAt] DATETIME2 NOT NULL,
        [EndAt] DATETIME2 NOT NULL,
        CONSTRAINT [FK_Shifts_AspNetUsers_EmployeeId] FOREIGN KEY([EmployeeId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_Shifts_EmployeeId] ON [Shifts]([EmployeeId]);
END;

IF OBJECT_ID(N'[ShiftSwapRequests]', N'U') IS NULL
BEGIN
    CREATE TABLE [ShiftSwapRequests](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [SourceShiftId] INT NOT NULL,
        [TargetShiftId] INT NOT NULL,
        [RequestedByEmployeeId] NVARCHAR(MAX) NOT NULL,
        [Status] NVARCHAR(32) NOT NULL,
        [ApprovedBy] NVARCHAR(450) NULL,
        [Comment] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        [DecidedAt] DATETIME2 NULL,
        CONSTRAINT [FK_ShiftSwapRequests_Shifts_SourceShiftId] FOREIGN KEY([SourceShiftId]) REFERENCES [Shifts]([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ShiftSwapRequests_Shifts_TargetShiftId] FOREIGN KEY([TargetShiftId]) REFERENCES [Shifts]([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_ShiftSwapRequests_SourceShiftId] ON [ShiftSwapRequests]([SourceShiftId]);
    CREATE INDEX [IX_ShiftSwapRequests_TargetShiftId] ON [ShiftSwapRequests]([TargetShiftId]);
END;

IF OBJECT_ID(N'[AttendanceEntries]', N'U') IS NULL
BEGIN
    CREATE TABLE [AttendanceEntries](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [EmployeeId] NVARCHAR(450) NOT NULL,
        [WorkDate] DATETIME2 NOT NULL,
        [ClockInAt] DATETIME2 NOT NULL,
        [ClockOutAt] DATETIME2 NULL,
        [TotalWorkedHours] DECIMAL(18,2) NOT NULL,
        CONSTRAINT [FK_AttendanceEntries_AspNetUsers_EmployeeId] FOREIGN KEY([EmployeeId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AttendanceEntries_EmployeeId] ON [AttendanceEntries]([EmployeeId]);
END;

IF OBJECT_ID(N'[AttendanceRequests]', N'U') IS NULL
BEGIN
    CREATE TABLE [AttendanceRequests](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [EmployeeId] NVARCHAR(450) NOT NULL,
        [Type] INT NOT NULL,
        [Status] NVARCHAR(32) NOT NULL,
        [RequestedForDate] DATETIME2 NOT NULL,
        [Reason] NVARCHAR(500) NULL,
        [ManagerComment] NVARCHAR(500) NULL,
        [DecidedBy] NVARCHAR(450) NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        [DecidedAt] DATETIME2 NULL,
        CONSTRAINT [FK_AttendanceRequests_AspNetUsers_EmployeeId] FOREIGN KEY([EmployeeId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AttendanceRequests_EmployeeId] ON [AttendanceRequests]([EmployeeId]);
END;

IF OBJECT_ID(N'[AttendancePenalties]', N'U') IS NULL
BEGIN
    CREATE TABLE [AttendancePenalties](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [EmployeeId] NVARCHAR(450) NOT NULL,
        [RuleCode] NVARCHAR(100) NOT NULL,
        [Points] INT NOT NULL,
        [Reason] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        [AttendanceEntryId] INT NULL,
        CONSTRAINT [FK_AttendancePenalties_AspNetUsers_EmployeeId] FOREIGN KEY([EmployeeId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AttendancePenalties_AttendanceEntries_AttendanceEntryId] FOREIGN KEY([AttendanceEntryId]) REFERENCES [AttendanceEntries]([Id])
    );
    CREATE INDEX [IX_AttendancePenalties_EmployeeId] ON [AttendancePenalties]([EmployeeId]);
    CREATE INDEX [IX_AttendancePenalties_AttendanceEntryId] ON [AttendancePenalties]([AttendanceEntryId]);
END;

IF OBJECT_ID(N'[AuditLogs]', N'U') IS NULL
BEGIN
    CREATE TABLE [AuditLogs](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Action] NVARCHAR(100) NOT NULL,
        [EntityName] NVARCHAR(100) NOT NULL,
        [EntityId] NVARCHAR(100) NOT NULL,
        [PerformedBy] NVARCHAR(450) NULL,
        [PerformedAt] DATETIME2 NOT NULL,
        [DetailsJson] NVARCHAR(MAX) NULL
    );
END;

IF NOT EXISTS (SELECT 1 FROM [SystemRules] WHERE [Code] = 'FLEXIBLE_SCHEDULE')
INSERT INTO [SystemRules]([Name],[Code],[IsEnabled],[ValueJson],[UpdatedAt],[UpdatedBy]) VALUES
('Flexible Schedule','FLEXIBLE_SCHEDULE',0,'{"AllowedStartFrom":"08:00","AllowedStartTo":"10:00","MinRequiredHours":8}',SYSUTCDATETIME(),'bootstrap');

IF NOT EXISTS (SELECT 1 FROM [SystemRules] WHERE [Code] = 'LATE_ARRIVAL')
INSERT INTO [SystemRules]([Name],[Code],[IsEnabled],[ValueJson],[UpdatedAt],[UpdatedBy]) VALUES
('Late Arrival','LATE_ARRIVAL',0,'{"GraceMinutes":10,"PointsPerLate":1}',SYSUTCDATETIME(),'bootstrap');

IF NOT EXISTS (SELECT 1 FROM [SystemRules] WHERE [Code] = 'SHIFT_SWAP')
INSERT INTO [SystemRules]([Name],[Code],[IsEnabled],[ValueJson],[UpdatedAt],[UpdatedBy]) VALUES
('Shift Swap','SHIFT_SWAP',1,NULL,SYSUTCDATETIME(),'bootstrap');
";

            await context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
