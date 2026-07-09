namespace Sample;

public class Class1
{

	public void TestStringInterpolation()
	{
		var name = "World";
		var greeting = string.Format("Hello, {0}!", name);
		Console.WriteLine(greeting);
	}

	public void TestUsingDeclaration()
	{
		using (var resource = new SomethingDisposable())
		{
			// Use the resource
		}
	}

	public void TestCollectionInitializer()
	{
		List<int> numbers = [1, 2, 3, 4, 5];
		foreach (var number in numbers)
		{
			Console.WriteLine(number);
		}
	}

}


public class SomethingDisposable : IDisposable
{
		public void Dispose()
		{
				// Cleanup resources
		}
}
