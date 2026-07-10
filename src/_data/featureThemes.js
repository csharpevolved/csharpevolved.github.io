module.exports = [
  {
    id: "foundations",
    label: "Language foundations",
    description:
      "Start with the syntax and everyday language features that make modern C# easier to read, write, and organize.",
    slugs: [
      "attributes-and-reflection",
      "var",
      "auto-implemented-properties",
      "named-and-optional-parameters",
      "file-local-types",
      "string-interpolation",
      "expression-bodied-members",
      "using-static-directives",
      "file-scoped-namespaces",
      "global-using-directives",
      "top-level-statements"
    ]
  },
  {
    id: "data-modeling",
    label: "Data modeling",
    description:
      "Follow the features that shape objects, records, tuples, and collection-friendly APIs into concise models.",
    slugs: [
      "anonymous-types",
      "object-and-collection-initializers",
      "tuples-and-deconstruction",
      "init-accessors",
      "records",
      "with-expressions",
      "required-members",
      "primary-constructors",
      "collection-expressions"
    ]
  },
  {
    id: "queries-and-composition",
    label: "Queries and composition",
    description:
      "Move from delegates into lambdas, LINQ, extensions, and reusable abstraction patterns for expressive APIs.",
    slugs: [
      "func-and-action-delegates",
      "lambda-expressions",
      "extension-methods",
      "linq",
      "local-functions",
      "default-interface-members",
      "static-abstract-interface-members"
    ]
  },
  {
    id: "control-flow-and-async",
    label: "Control flow and async",
    description:
      "Explore the features that keep branching, error handling, and asynchronous workflows more expressive and direct.",
    slugs: [
      "async-await",
      "async-streams",
      "exception-filters",
      "pattern-matching",
      "switch-expressions",
      "list-patterns"
    ]
  },
  {
    id: "performance-and-memory",
    label: "Performance and memory",
    description:
      "Track the performance-oriented features that reduce copying, improve locality, and open lower-level control.",
    slugs: [
      "out-ref-in-parameters",
      "ref-returns",
      "ref-structs",
      "span-and-readonlyspan",
      "range-and-index-operators"
    ]
  },
  {
    id: "safety-and-maintainability",
    label: "Safety and maintainability",
    description:
      "Browse the features that help modern C# code stay safer, cleaner, and easier to maintain over time.",
    slugs: [
      "null-coalescing-and-assignment",
      "nameof-callerargumentexpression",
      "using-declarations",
      "nullable-reference-types"
    ]
  }
];
