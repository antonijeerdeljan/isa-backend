namespace ISA.Application.API.Models.Requests;

public record InitialPasswordChangeModel(string email, string oldPassword, string newPassword);