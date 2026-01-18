using System.Threading.Tasks;

namespace AlarmaSueño.Core
{
    public interface IPhraseProvider
    {
        Task LoadQuotesAsync();
        string ObtenerFrase();
    }
}
