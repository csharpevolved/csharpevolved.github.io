public class LocalFunctionsAdvanced
{
    // Recursive local function
    public long Factorial(int n)
    {
        if (n < 0) throw new ArgumentException("n must be non-negative");

        return FactorialLocal(n);

        long FactorialLocal(int x) => x <= 1 ? 1 : x * FactorialLocal(x - 1);
    }

    // Fibonacci with memoization using local function
    public int Fibonacci(int n)
    {
        var cache = new Dictionary<int, int>();

        return FibLocal(n);

        int FibLocal(int x)
        {
            if (x <= 1) return x;
            if (cache.TryGetValue(x, out int cached)) return cached;

            int result = FibLocal(x - 1) + FibLocal(x - 2);
            cache[x] = result;
            return result;
        }
    }

    // Local functions capturing outer variables (closure)
    public Func<int, int> CreateMultiplier(int factor)
    {
        // Local function captures 'factor' from enclosing scope
        int Multiply(int x) => x * factor;

        // Return as delegate
        return Multiply;
    }

    // Multiple local functions with interdependencies
    public void GenerateReport(List<int> data)
    {
        var sum = 0;
        var max = int.MinValue;

        ProcessData();
        PrintSummary();

        void ProcessData()
        {
            foreach (var value in data)
            {
                sum += value;
                if (value > max) max = value;
            }
        }

        void PrintSummary()
        {
            double average = data.Count > 0 ? (double)sum / data.Count : 0;
            Console.WriteLine($"Sum: {sum}, Average: {average:F2}, Max: {max}");
        }
    }

    // TreeNode for recursive example
    public class TreeNode
    {
        public int Value { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
    }

    // Binary tree traversal using local recursion
    public List<int> InOrderTraversal(TreeNode root)
    {
        var result = new List<int>();

        Traverse(root);
        return result;

        void Traverse(TreeNode node)
        {
            if (node == null) return;

            Traverse(node.Left);
            result.Add(node.Value);
            Traverse(node.Right);
        }
    }
}
