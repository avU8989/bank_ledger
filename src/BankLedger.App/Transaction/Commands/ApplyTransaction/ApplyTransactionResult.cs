//so we get the transaction id as a result of applying the transaction, we can use it for logging or other purposes
public sealed record ApplyTransactionResult(Guid TransactionId);