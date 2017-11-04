using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskTracker.DataAccess.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DECLARE @count int = 0;
SELECT @count = COUNT(*) FROM dbo.Task
IF (@count = 0)
BEGIN
DECLARE @index int = 0
DECLARE @current datetime = GETUTCDATE(); 
WHILE (@index < 25000)
BEGIN
	INSERT dbo.Task (Name, Description, Priority, Status, Duration, Added, Edited) VALUES 
	('Read assignment', 'Read the assignment<b>attentively<b>', 2, 0, '00:10:00', @current, @current),
	('Impress others', 'Impress other <i>members</i> of the team', 3, 0, '00:05:35', @current, @current),
	('Build the app', 'Build the app using <strong>Visual Studio 2017</strong>', 4, 0, '04:00:00', @current, @current),
	('Get hired', 'Finaly get <em>hired</em> to the job of your dream', 1, 0, '14:04:00', @current, @current)
	SET @index = @index + 1
END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE dbo.Task");
        }
    }
}
