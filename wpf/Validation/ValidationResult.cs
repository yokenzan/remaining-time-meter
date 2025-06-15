// <copyright file="ValidationResult.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace RemMeter.Validation
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="isValid">Whether the validation passed.</param>
        /// <param name="errorMessages">Collection of error messages if validation failed.</param>
        public ValidationResult(bool isValid, IEnumerable<string>? errorMessages = null)
        {
            this.IsValid = isValid;
            this.ErrorMessages = errorMessages?.ToList() ?? new List<string>();
        }

        /// <summary>
        /// Gets a value indicating whether the validation passed.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the collection of error messages if validation failed.
        /// </summary>
        public IReadOnlyList<string> ErrorMessages { get; }

        /// <summary>
        /// Gets the first error message, or null if no errors.
        /// </summary>
        public string? FirstErrorMessage => this.ErrorMessages.FirstOrDefault();

        /// <summary>
        /// Creates a successful validation result.
        /// </summary>
        /// <returns>A validation result indicating success.</returns>
        public static ValidationResult Success()
        {
            return new ValidationResult(true);
        }

        /// <summary>
        /// Creates a failed validation result with a single error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>A validation result indicating failure.</returns>
        public static ValidationResult Failure(string errorMessage)
        {
            return new ValidationResult(false, new[] { errorMessage });
        }

        /// <summary>
        /// Creates a failed validation result with multiple error messages.
        /// </summary>
        /// <param name="errorMessages">The collection of error messages.</param>
        /// <returns>A validation result indicating failure.</returns>
        public static ValidationResult Failure(IEnumerable<string> errorMessages)
        {
            return new ValidationResult(false, errorMessages);
        }
    }
}
