﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.crypto.utilities;
using ut = magic.lambda.crypto.utilities;

namespace magic.lambda.crypto.slots
{
    /// <summary>
    /// [crypto.encrypt] slot that signs and encrypts the specified
    /// content using the spcified arguments.
    /// </summary>
    [Slot(Name = "crypto.encrypt")]
    public class Encrypt : ISlot
    {
        /// <summary>
        /// Implementation of slot.
        /// </summary>
        /// <param name="signaler">Signaler invoking slot.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Retrieving arguments.
            var content = ut.Utilities.GetContent(input);
            var signingKey = ut.Utilities.GetKeyFromArguments(input, "signing-key");
            var encryptionKey = ut.Utilities.GetKeyFromArguments(input, "encryption-key");
            var signingKeyFingerprint = ut.Utilities.GetFingerprint(input, "signing-key-fingerprint");
            var raw = input.Children.FirstOrDefault(x => x.Name == "raw")?.GetEx<bool>() ?? false;
            var seed = input.Children.FirstOrDefault(x => x.Name == "seed")?.GetEx<string>();

            // House cleaning.
            input.Clear();
            input.Value = null;

            // Creating an encrypter.
            var encrypter = new Encrypter(
                encryptionKey,
                signingKey,
                signingKeyFingerprint,
                seed);

            // Signing and encrypting content.
            var rawResult = encrypter.SignAndEncrypt(content);

            // Returning results to caller.
            input.Value = raw ? rawResult : (object)Convert.ToBase64String(rawResult);
        }
    }
}