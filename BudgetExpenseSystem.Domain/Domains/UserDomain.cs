using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Domain.Interfaces;
using BudgetExpenseSystem.Model.Dto.Requests;
using BudgetExpenseSystem.Model.Dto.Response;
using BudgetExpenseSystem.Model.Models;
using BudgetExpenseSystem.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BudgetExpenseSystem.Domain.Domains;

public class UserDomain : IUserDomain
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IUserRepository _userRepository;
	private readonly IRoleRepository _roleRepository;
	private readonly string _jwtSecretKey;

	public UserDomain(IUnitOfWork unitOfWork, IUserRepository userRepository, IConfiguration configuration,
		IRoleRepository roleRepository)
	{
		_unitOfWork = unitOfWork;
		_roleRepository = roleRepository;
		_userRepository = userRepository;
		_jwtSecretKey = configuration["JwtSettings:SecretKey"]
		                ?? throw new Exception("JwtSettings:SecretKey not found in configuration");
	}

	public async Task<List<User>> GetAllAsync()
	{
		return await _userRepository.GetAllAsync();
	}

	public async Task<User> GetByIdAsync(int id)
	{
		var user = await _userRepository.GetByIdAsync(id);
		if (user == null) throw new NotFoundException($"User Id: {id} not found");

		return user;
	}

	public async Task<User> RegisterUserAsync(UserRequest userRequest)
	{
		var existingUser = await _userRepository.GetUserEmailAsync(userRequest.Email);
		if (existingUser is not null) throw new Exception($"User with email {userRequest.Email} already exists.");

		var salt = GenerateSalt();
		var passwordHash = HashPassword(userRequest.Password, salt);

		var role = await _roleRepository.GetByIdAsync(userRequest.RoleId);
		if (role == null) throw new NotFoundException($"Role with Id {userRequest.RoleId} not found");

		var newUser = new User
		{
			Email = userRequest.Email,
			PasswordHash = passwordHash,
			PasswordSalt = salt,
			Role = role
		};


		_userRepository.AddAsync(newUser);
		await _unitOfWork.SaveAsync();

		return newUser;
	}

	public async Task<TokenResponse> LoginUserAsync(string email, string password)
	{
		var user = await _userRepository.GetUserEmailAsync(email);
		if (user == null) throw new NotFoundException("User doesn't exist");

		var role = await _roleRepository.GetByIdAsync(user.RoleId);
		if (role == null) throw new NotFoundException($"Role Id: {user.RoleId} not found");

		var hashedPassword = HashPassword(password, user.PasswordSalt);
		if (hashedPassword != user.PasswordHash) throw new BadRequestException("Invalid password");

		var token = GenerateJwtToken(user);

		return new TokenResponse
		{
			Token = token,
		};
	}

	public async Task DeleteAsync(int id)
	{
		var user = await _userRepository.GetByIdAsync(id);
		if (user == null) throw new NotFoundException($"User Id: {id} not found");

		await _userRepository.DeleteAsync(id);
		await _unitOfWork.SaveAsync();
	}

	private static string GenerateSalt()
	{
		var salt = new byte[16];
		RandomNumberGenerator.Fill(salt);
		return Convert.ToBase64String(salt);
	}

	private string GenerateJwtToken(User user)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new("email", user.Email),
			new(ClaimTypes.Role, user.Role.Name)
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			null,
			null,
			claims,
			expires: DateTime.Now.AddHours(5),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	private static string HashPassword(string password, string salt)
	{
		using (var hmac = new HMACSHA512(Convert.FromBase64String(salt)))
		{
			var passwordBytes = Encoding.UTF8.GetBytes(password);
			var hash = hmac.ComputeHash(passwordBytes);
			return Convert.ToBase64String(hash);
		}
	}
}
