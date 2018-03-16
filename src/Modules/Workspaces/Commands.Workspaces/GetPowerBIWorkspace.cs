﻿/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<Group>))]
    public class GetPowerBIWorkspace : PowerBICmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Get;

        #region Parameters

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        #endregion

        protected override void ExecuteCmdlet()
        {
            PowerBIClient client = null;
            var token = this.Authenticator.Authenticate(this.Profile, this.Logger, this.Settings);
            if (Uri.TryCreate(this.Profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                client = new PowerBIClient(baseUri, new TokenCredentials(token.AccessToken));
            }
            else
            {
                client = new PowerBIClient(new TokenCredentials(token.AccessToken));
            }

            var workspaces = this.Scope.Equals(PowerBIUserScope.Individual) ? client.Groups.GetGroups() : client.Groups.GetGroupsAsAdmin();
            this.Logger.WriteObject(workspaces.Value, true);
        }
    }
}