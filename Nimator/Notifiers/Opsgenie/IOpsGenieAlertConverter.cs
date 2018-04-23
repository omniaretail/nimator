namespace Nimator.Notifiers.Opsgenie
{
    /// <summary>
    /// Converts an <see cref="INimatorResult"/> to an <see cref="OpsGenieAlertRequest"/>.
    /// </summary>
    public interface IOpsGenieAlertConverter
    {
        /// <summary>
        /// Creates an <see cref="OpsGenieAlertRequest"/> from an <see cref="INimatorResult"/>.
        /// </summary>
        /// <param name="result">The <see cref="INimatorResult"/> to convert</param>
        /// <returns>The converted <see cref="OpsGenieAlertRequest"/></returns>
        OpsGenieAlertRequest Convert(INimatorResult result);
    }
}
