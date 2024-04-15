using API.DTO;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/users")] 
    [ApiController]
    public class UserController: BaseApiController
    {
        private readonly StoreContext _authContext;

    public UserController(StoreContext context)
    {
        _authContext = context;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            
            if(loginDto==null){
                return BadRequest();
            }

            var user = await _authContext.Users
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return BadRequest(new { Message = "Incorrect password" });
            }
            
            user.Token = CreateToken(user);
            
            return Ok(
                new{
                    
                    Message = "Login successful",
                    Token = user.Token
                }
            );
        }

    [HttpPost("register")]
    
    //RREGULLO USER TO REGISTERDTO, PRANON PASSWORDE TE THJESHTA, SHIH REGEXAT
    public async Task<IActionResult> Register([FromBody] User user){
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
            
        }
        
        if(user == null){
            return NotFound();
        }

        var emailExists = await _authContext.Users.AnyAsync(x=> x.Email == user.Email);
        if(emailExists){
            return BadRequest(new {Message = "Email already exists"});
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.Password = hashedPassword;
        

        await _authContext.AddAsync(user);
        await _authContext.SaveChangesAsync();

        return Ok(new {
            Message = "Register successful"
        });
    }

    [HttpGet("{Id}")]
    public IActionResult GetUser(int Id)
    {
        var user = _authContext.GetUserById(Id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet] 
    public IActionResult GetAllUsers()
    {
        var users = _authContext.GetAllUsers();
        return Ok(users);
    }
    
    [HttpPut("{Id}")]
    public IActionResult UpdateUser(int Id, [FromBody] User updatedUser)
    {
        var existingUser = _authContext.Users.Find(Id);
        if (existingUser == null)
        {
            return NotFound($"User with ID {Id} not found.");
        }

        existingUser.FullName = updatedUser.FullName ?? existingUser.FullName;
        existingUser.Email = updatedUser.Email ?? existingUser.Email;
        existingUser.Address = updatedUser.Address ?? existingUser.Address;
        existingUser.Password = updatedUser.Password ?? existingUser.Password;
        existingUser.PhoneNumber = updatedUser.PhoneNumber ?? existingUser.PhoneNumber;

        // Save the changes to the database
        _authContext.SaveChanges();

        return Ok(existingUser);
    }

    [HttpDelete("{Id}")]
    public IActionResult DeleteUser(int Id){

            try
            {
                _authContext.DeleteUserById(Id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"An error occurred while deleting the user. Please try again later.");
            }

        }

        private string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("MyVerySecretKeyNobodyKnowsAbout...Shhhh");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("FullName", user.FullName),
                    new Claim("Email", user.Email),
                    new Claim("Id", user.Id.ToString()) 
                }),
                Expires = DateTime.UtcNow.AddDays(1), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }

} 
