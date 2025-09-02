// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

namespace WixToolset.BuildTasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public sealed class CreatePackageReferenceDefineConstants : Task
    {
        [Required]
        public ITaskItem[] ResolvedPackageReferences { get; set; }

        [Output]
        public ITaskItem[] DefineConstants { get; private set; }

        public override bool Execute()
        {
            var defineConstants = new SortedDictionary<string, string>();

            foreach (var packageReference in this.ResolvedPackageReferences)
            {
                this.AddDefineConstantsForResolvedReference(defineConstants, packageReference);
            }

            this.DefineConstants = defineConstants.Select(define => new TaskItem(define.Key + "=" + define.Value)).ToArray<ITaskItem>();

            return true;
        }

        private void AddDefineConstantsForResolvedReference(IDictionary<string, string> defineConstants, ITaskItem packageReference)
        {
            var packageName = packageReference.GetMetadata("Name");
            if (String.IsNullOrWhiteSpace(packageName))
            {
                packageName = packageReference.GetMetadata("Identity");
            }

            var path = packageReference.GetMetadata("Path");
            if (!String.IsNullOrWhiteSpace(path))
            {
                defineConstants[packageName + ".PackageDir"] = path;
            }
        }
    }
}
