Extension members still can't access private or internal state — they operate entirely through the type's public surface, just like the old static-method pattern. If you need access to private fields, you still need to modify the original type or use a wrapper.

Overuse can make a codebase harder to navigate: members that appear to live on a type aren't in that type's source file, so developers unfamiliar with your extension declarations may struggle to find where behavior is defined. Keep extension blocks focused, name them clearly, and place them close to the code that uses them.

The `extension` block syntax requires C# 14 and .NET 10; projects targeting earlier runtimes or language versions must stay with the classic static-class pattern.
