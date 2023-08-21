using Npgsql;
using Npgsql.Replication;

var connStr =
    "PORT = 5432; HOST = localhost; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'postgres'; PASSWORD = 'Password12!'; USER ID = 'postgres'";

var cts = new CancellationTokenSource();
var ct = cts.Token;

await using var dataSource = NpgsqlDataSource.Create(connStr);
var pbName = $"base_pb_{Guid.NewGuid():N}";
var slotName = Guid.NewGuid().ToString("N");
var cmd = dataSource.CreateCommand($"CREATE PUBLICATION {pbName} FOR TABLE events;");
await cmd.ExecuteNonQueryAsync();
var lConn = new LogicalReplicationConnection(connStr);
await lConn.Open();
var result =
    await lConn.CreatePgOutputReplicationSlot(slotName, slotSnapshotInitMode: LogicalSlotSnapshotInitMode.Export);
var snapshotName = result.SnapshotName;
var connection = new NpgsqlConnection(connStr);
await connection.OpenAsync();
connection.g;

