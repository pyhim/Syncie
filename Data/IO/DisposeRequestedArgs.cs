namespace Syncie.Data.IO;

public class DisposeRequestedArgs : EventArgs
{
    public Guid TrackingId { get; set; }
}