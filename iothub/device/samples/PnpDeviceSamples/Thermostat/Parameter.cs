﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using CommandLine;

namespace Thermostat
{
    /// <summary>
    /// Parameters for the application supplied via command line arguments. It default to environment variables..
    /// </summary>
    internal class Parameters
    {
        [Option(
            's',
            "DeviceSecurityType",
            HelpText = "(Required) The provisioning type that will be used for provisioning the device for the sample. Possible values include dps, connectionString (case-insensitive)." +
            "\nDefaults to environment variable \"IOTHUB_DEVICE_SECURITY_TYPE\".")]
        public string DeviceSecurityType { get; set; } = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_SECURITY_TYPE");

        [Option(
            'p',
            "PrimaryConnectionString",
            HelpText = "(Required if DeviceSecurityType is \"connectionString\"). \nThe primary connection string for the device to simulate." +
            "\nDefaults to environment variable \"IOTHUB_DEVICE_CONNECTION_STRING\".")]
        public string PrimaryConnectionString { get; set; } = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_CONNECTION_STRING");

        [Option(
            'e',
            "DpsEndpoint",
            HelpText = "(Required if DeviceSecurityType is \"dps\"). \nThe DPS endpoint to use during device provisioning." +
            "\nDefaults to environment variable \"IOTHUB_DEVICE_DPS_ENDPOINT\".")]
        public string DpsEndpoint { get; set; } = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_DPS_ENDPOINT");

        [Option(
            'i',
            "DpsIdScope",
            HelpText = "(Required if DeviceSecurityType is \"dps\"). \nThe DPS ID Scope to use during device provisioning." +
            "\nDefaults to environment variable \"IOTHUB_DEVICE_DPS_ID_SCOPE\".")]
        public string DpsIdScope { get; set; } = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_DPS_ID_SCOPE");

        [Option(
            'd',
            "DeviceId",
            HelpText = "(Required if DeviceSecurityType is \"dps\"). \nThe device registration Id to use during device provisioning." +
            "\nDefaults to environment variable \"IOTHUB_DEVICE_DPS_DEVICE_ID\".")]
        public string DeviceId { get; set; } = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_DPS_DEVICE_ID");

        [Option(
            'k',
            "DeviceSymmetricKey",
            HelpText = "(Required if DeviceSecurityType is \"dps\"). \nThe device symmetric key to use during device provisioning." +
            "\nDefaults to environment variable \"IOTHUB_DEVICE_DPS_DEVICE_KEY\".")]
        public string DeviceSymmetricKey { get; set; } = Environment.GetEnvironmentVariable("IOTHUB_DEVICE_DPS_DEVICE_KEY");

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(DeviceSecurityType))
            {
                throw new ArgumentNullException(nameof(DeviceSecurityType), "Device provisioning type needs to be specified, please set the environment variable \"IOTHUB_DEVICE_SECURITY_TYPE\"" +
                    "or pass in \"-s | --DeviceSecurityType\" through command line.");
            }

            return (DeviceSecurityType.ToLowerInvariant()) switch
            {
                "dps" => !string.IsNullOrWhiteSpace(DpsEndpoint)
                        && !string.IsNullOrWhiteSpace(DpsIdScope)
                        && !string.IsNullOrWhiteSpace(DeviceId)
                        && !string.IsNullOrWhiteSpace(DeviceSymmetricKey),
                "connectionstring" => !string.IsNullOrWhiteSpace(PrimaryConnectionString),
                _ => throw new ArgumentException($"Unrecognized value for device provisioning received: {DeviceSecurityType}." +
                        $" It should be either \"dps\" or \"connectionString\" (case-insensitive)."),
            };
        }
    }
}
