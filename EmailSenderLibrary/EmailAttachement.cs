namespace CricketTournamentAdmin.Core.Models;

/// <summary>
/// Represents a transport-agnostic email attachment.
/// </summary>
/// <remarks>
/// Use this class to pass attachments to email services without coupling the core model
/// to ASP.NET Core types such as <c>IFormFile</c>. The attachment content is exposed
/// as a <see cref="Stream"/> so callers can provide files from uploads, disk, memory,
/// or cloud storage. Decide and document who is responsible for disposing the stream.
/// </remarks>
public sealed class EmailAttachment
{
    /// <summary>
    /// The file name that will be shown to the recipient.
    /// </summary>
    /// <value>
    /// Defaults to an empty string. Should include an extension when applicable (for example, "invoice.pdf").
    /// </value>
    public string FileName { get; init; } = string.Empty;

    /// <summary>
    /// The MIME content type of the attachment.
    /// </summary>
    /// <value>
    /// Defaults to "application/octet-stream". Use a specific MIME type when known (for example, "application/pdf" or "image/png").
    /// </value>
    public string ContentType { get; init; } = "application/octet-stream";

    /// <summary>
    /// The stream that contains the attachment content.
    /// </summary>
    /// <value>
    /// Defaults to <see cref="Stream.Null"/>. The stream should be readable. If the stream supports seeking,
    /// its position should be set to the beginning before sending.
    /// </value>
    public Stream ContentStream { get; init; } = Stream.Null;

    /// <summary>
    /// The length of the content stream in bytes when available.
    /// </summary>
    /// <remarks>
    /// Accessing <see cref="Length"/> may throw for non-seekable streams in some environments; this property
    /// catches exceptions and returns 0 when the length cannot be determined.
    /// </remarks>
    public long Length => ContentStream?.Length ?? 0;
}
