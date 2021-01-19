﻿namespace SharedCode
{
    /// <summary>
    ///     Shared class used for Launcher preferences
    /// </summary>
    public class LauncherPrefs
    {
        public enum ReleaseType
        {
            Release,
            Beta,
            Ci
        }

        public ReleaseType ReleaseBranch { get; set; } = ReleaseType.Release;
        public int ReleaseInstalled { get; set; } = 0;
        public ReleaseType ReleaseTypeInstalled { get; set; } = ReleaseType.Release;
    }
}