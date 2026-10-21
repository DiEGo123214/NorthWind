using System.Threading.Tasks;

namespace NorthWind.Entities.Interfaces;

public interface IMailService
{
    Task SendMailToAdministrator(string subject, string body);
}
