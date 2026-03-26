using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PAW3.Architecture.Helpers;
using System.Globalization;
using System.Text;

namespace PAW3.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FormattersController : Controller
  {

    private ITextHelper _textHelper;
    private IDatesHelper _datesHelper;
    private IGeneralHelper _generalHelper;
    private INumbersHelper _numberHelper;
    private IValidationHelper _validationHelper;

    public FormattersController(ITextHelper textHelper, IDatesHelper datesHelper, IGeneralHelper generalHelper, INumbersHelper numbersHelper, IValidationHelper validationHelper) {
      _textHelper = textHelper;
      _datesHelper = datesHelper;
      _generalHelper = generalHelper;
      _numberHelper = numbersHelper;
      _validationHelper = validationHelper;
    }

    

    [HttpGet("/string/ToTitleCase")]
    public async Task<ActionResult<string>> GetToTitleCase([FromQuery] string wording)
    {
      if(wording.IsNullOrEmpty()) return BadRequest();
      return Ok(_textHelper.ToTitleCase(wording, culture: CultureInfo.InvariantCulture));
    }

    [HttpGet("/dates/CalculateAge")]
    public async Task<JsonResult> GetCalculateAge([FromQuery] DateTime bday)
    {
      var age = _datesHelper.CalculateAge(bday);
      return new JsonResult(new { age })
      {
        StatusCode = 200
      }; 
    }

    [HttpGet("/general/FormatPhone")]
    public async Task<ActionResult<string>> GetFormattedPhone([FromQuery] string phoneNumber)
    {
      var formattedNumber = _generalHelper.FormatPhone(phoneNumber);
      if(phoneNumber.IsNullOrEmpty())
        return BadRequest();
      return Ok(formattedNumber);
    }

    [HttpGet("/number/FormatPercentage")]
    public async Task<ActionResult<string>> GetFormatPercentage([FromQuery] double number, [FromQuery] int decimals)
    {
      var formattedNumber = _numberHelper.FormatPercentage(number, decimals);
      return Ok(formattedNumber);
    }

    [HttpGet("/validations/isEmailValid")]
    public async Task<ActionResult<bool>> GetIsEmailValid([FromQuery] string email)
    {
      var isValid = _validationHelper.IsValidEmail(email);
      if(email.IsNullOrEmpty() ) return BadRequest();
      return Ok(isValid);
    }

  }
}
