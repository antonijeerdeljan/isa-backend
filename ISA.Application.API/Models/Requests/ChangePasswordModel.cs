namespace ISA.Application.API.Models.Requests;

public record ChangePasswordModel(string currentPassword, string newPassword);