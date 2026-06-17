var userName = "Avery";
var orderTotal = 27.5m;

var message = string.Format(
    "Customer {0} placed an order totaling {1:C}.",
    userName,
    orderTotal
);
