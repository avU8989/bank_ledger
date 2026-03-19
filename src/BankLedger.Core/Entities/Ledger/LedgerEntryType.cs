namespace BankLedger.Core.Ledger;

public enum LedgerEntryType
{
    Deposit,
    Withdrawal,
    TransferIn,
    TransferOut,
    Fee,
    OverdraftFee
}