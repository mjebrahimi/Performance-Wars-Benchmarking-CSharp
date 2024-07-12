namespace MySqlBulkInsertExcel_Benchmark;

public class NonDisposableStream(Stream baseStream) : Stream
{
    private readonly Stream _baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));

    public override ValueTask DisposeAsync()
    {
        // Do nothing to prevent disposal
        return ValueTask.CompletedTask;
    }

    protected override void Dispose(bool disposing)
    {
        // Do nothing to prevent disposal
    }

    public override void Close()
    {
        // Do nothing to prevent closure
    }

    public override bool CanRead => _baseStream.CanRead;
    public override bool CanSeek => _baseStream.CanSeek;
    public override bool CanWrite => _baseStream.CanWrite;
    public override long Length => _baseStream.Length;
    public override long Position { get => _baseStream.Position; set => _baseStream.Position = value; }

    public override void Flush() => _baseStream.Flush();
    public override int Read(byte[] buffer, int offset, int count) => _baseStream.Read(buffer, offset, count);
    public override long Seek(long offset, SeekOrigin origin) => _baseStream.Seek(offset, origin);
    public override void SetLength(long value) => _baseStream.SetLength(value);
    public override void Write(byte[] buffer, int offset, int count) => _baseStream.Write(buffer, offset, count);
}