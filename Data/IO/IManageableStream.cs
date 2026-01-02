namespace Syncie.Data.IO;

public interface IManageableStream : IStream
{
    /// <summary>
    /// The last time since any action has been performed with the stream.
    /// </summary>
    DateTime LastActivity { get; }
    
    /// <summary>
    /// The tracking ID given by the manager of this object.
    /// </summary>
    Guid TrackingId { get; }
}