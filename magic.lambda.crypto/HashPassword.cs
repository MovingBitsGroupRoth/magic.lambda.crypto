﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Collections.Generic;
using bc = BCrypt.Net;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.crypto
{
    [Slot(Name = "crypto.password.hash")]
    public class HashPassword : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            input.Value = bc.BCrypt.HashPassword(input.GetEx<string>());
        }
    }
}