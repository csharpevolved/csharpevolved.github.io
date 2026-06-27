public class CombinedRangeAndIndex
{
    public void PaginationExample()
    {
        var allItems = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int pageSize = 3;
        int pageNumber = 1; // 0-indexed

        // Extract one page using range
        var page = allItems[(pageNumber * pageSize)..((pageNumber + 1) * pageSize)];
        // page = [4, 5, 6]
    }

    public void LogRotationExample()
    {
        var logLines = new[]
        {
            "[INFO] App started",
            "[DEBUG] Loading config",
            "[INFO] Config loaded",
            "[WARN] Cache miss",
            "[ERROR] Connection failed",
            "[INFO] Retry attempt 1"
        };

        // Get all lines except the first and last (trim headers and summary)
        var mainContent = logLines[1..^1];

        // Get last 3 lines (recent activity)
        var recent = logLines[^3..];

        // Get errors and warnings (assuming they're at specific indices)
        var criticalLines = logLines[4..5]; // Just the error
    }

    public void DataValidationExample()
    {
        string csvLine = "123,John,Doe,john@example.com,2025-01-15";
        var fields = csvLine.Split(',');

        // Extract required fields using index from end
        string email = fields[^2];            // "john@example.com"
        string dateStr = fields[^1];          // "2025-01-15"

        // Extract name parts (first two fields after ID)
        string fullName = string.Join(" ", fields[1..3]); // "John Doe"

        // Validate: must have at least 5 fields
        bool valid = fields.Length >= 5;
    }

    public void SlidingWindowExample()
    {
        var values = new[] { 10, 20, 30, 40, 50, 60, 70, 80 };
        int windowSize = 3;

        // Calculate moving average
        double[] movingAverages = new double[values.Length - windowSize + 1];

        for (int i = 0; i < movingAverages.Length; i++)
        {
            var window = values[i..(i + windowSize)];
            movingAverages[i] = window.Average();
        }
        // movingAverages[0] = 20 (avg of 10, 20, 30)
        // movingAverages[1] = 30 (avg of 20, 30, 40)
    }
}
