using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;

namespace BudgetExpenseSystem.Domain.Domains;

public class AccountDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<Account> _accountRepository;

	public AccountDomain(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_accountRepository = _unitOfWork.GetRepository<Account>();
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
		var accout = await _accountRepository.GetByIdAsync(id);
		if (accout is null) throw new NotFoundException($"Account Id: {id} not found");

		await _accountRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}
}
