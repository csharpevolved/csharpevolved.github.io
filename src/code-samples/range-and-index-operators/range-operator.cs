public class RangeOperatorExamples
{
    public void ArrayRanges()
    {
        var numbers = new[] { 10, 20, 30, 40, 50, 60, 70, 80 };

        // Range from start
        int[] firstThree = numbers[..3];           // [10, 20, 30]

        // Range to end
        int[] lastThree = numbers[5..];            // [60, 70, 80]

        // Range in middle
        int[] middle = numbers[2..5];              // [30, 40, 50]

        // Full range (creates a copy)
        int[] copy = numbers[..];                  // all elements
    }

    public void StringRanges()
    {
        string path = "C:\\Users\\Documents\\file.txt";

        // Extract filename
        int lastSlash = path.LastIndexOf('\\');
        string fileName = path[(lastSlash + 1)..]; // "file.txt"

        // Extract extension
        int lastDot = path.LastIndexOf('.');
        string extension = path[lastDot..];        // ".txt"

        // Extract all but extension
        string nameOnly = path[..lastDot];         // "C:\\Users\\Documents\\file"
    }

    public void SpanRanges()
    {
        Span<byte> buffer = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };

        // Extract header (first 2 bytes)
        Span<byte> header = buffer[..2];

        // Extract payload (skip first 2 bytes)
        Span<byte> payload = buffer[2..];

        // Extract specific frame (zero-copy operation)
        Span<byte> frame = buffer[1..4];
    }
}
