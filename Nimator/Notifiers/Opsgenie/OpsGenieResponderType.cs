using System.Runtime.Serialization;

namespace Nimator.Notifiers.Opsgenie
{
    /// <summary>
    /// Represents the responder type in <see cref="OpsGenieResponder"/>.
    /// </summary>
    public enum OpsGenieResponderType
    {
        /// <summary>
        /// Represents a team.
        /// </summary>
        [EnumMember(Value = "team")]        
        Team = 1,
        
        /// <summary>
        /// Represents a user.
        /// </summary>
        [EnumMember(Value = "user")]
        User = 2,
        
        /// <summary>
        /// Represents an escalation.
        /// </summary>
        [EnumMember(Value = "escalation")]
        Escalation = 3,
        
        /// <summary>
        /// Represents a schedule.
        /// </summary>
        [EnumMember(Value = "schedule")]
        Schedule = 4,
    }
}
