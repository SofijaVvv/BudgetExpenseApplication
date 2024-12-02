using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class AccountDomain : IAccountDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAccountRepository _accountRepository;

	public AccountDomain(IUnitOfWork unitOfWork, IAccountRepository accountRepository)
	{
		_unitOfWork = unitOfWork;
		_accountRepository = accountRepository;
	}

	public async Task<List<Account>> GetAllAsync()
	{
		return await _accountRepository.GetAllAsync();
	}


	public async Task<Account> GetByIdAsync(int id)
	{
		var account = await _accountRepository.GetByIdAsync(id);
		if (account == null) throw new NotFoundException($"Account Id {id} not found");

		return account;
	}


	public async Task<Account> AddAsync(Account account)
	{
		_accountRepository.AddAsync(account);

		await _unitOfWork.SaveAsync();
		return account;
	}


	public async Task Update(int accountId, UpdateAccountRequest updateAccountRequest)
	{
		var account = await _accountRepository.GetByIdAsync(accountId);
		if (account is null) throw new NotFoundException($"Account Id: {accountId} not found");

		account.Balance = updateAccountRequest.Balance;

		await _unitOfWork.SaveAsync();
	}


	public async Task DeleteAsync(int id)
	{
		var account = await _accountRepository.GetByIdAsync(id);
		if (account is null) throw new NotFoundException($"Account Id: {id} not found");

		await _accountRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}
