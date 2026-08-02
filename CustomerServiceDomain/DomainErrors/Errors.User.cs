using ErrorOr;

namespace CustomerSupport.Domain.DomainErrors;

public static partial class Errors
{
	public static class User
	{
		public static Error FullNameIsRequired => Error.Validation(
			code: "User.FullNameIsRequired",
			description: "Full name is required.");

		public static Error EmailIsRequired => Error.Validation(
			code: "User.EmailIsRequired",
			description: "Email is required.");

		public static Error InvalidEmailFormat => Error.Validation(
			code: "User.InvalidEmailFormat",
			description: "Email format is invalid.");
	}
}