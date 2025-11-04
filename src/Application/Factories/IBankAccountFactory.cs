using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Factories;

public interface IBankAccountFactory
{
    BankAccount Create(BankAccountDto dto);
}