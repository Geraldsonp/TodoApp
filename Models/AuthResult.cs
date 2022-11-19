namespace TodoApp.Models;

public struct AuthResult
{
    public bool Succeded { get; }
    public string? ErrorMessage { get; }

    public AuthResult(bool Succeded, string? ErrorMessage)
    {
        this.Succeded = Succeded;
        this.ErrorMessage = ErrorMessage;
    }
}