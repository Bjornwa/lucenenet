/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

using Payload = Lucene.Net.Index.Payload;

namespace Lucene.Net.Analysis
{
	
	/// <summary>A TokenStream enumerates the sequence of tokens, either from
	/// fields of a document or from query text.
	/// <p>
	/// This is an abstract class.  Concrete subclasses are:
	/// <ul>
	/// <li>{@link Tokenizer}, a TokenStream
	/// whose input is a Reader; and
	/// <li>{@link TokenFilter}, a TokenStream
	/// whose input is another TokenStream.
	/// </ul>
    /// NOTE: subclasses must override {@link #next(Token)}.  It's
    /// also OK to instead override {@link #next()} but that
    /// method is now deprecated in favor of {@link #next(Token)}.
    /// </summary>
	
	public abstract class TokenStream
	{
		
		/// <summary>Returns the next token in the stream, or null at EOS.
		/// @deprecated The returned Token is a "full private copy" (not
		/// re-used across calls to next()) but will be slower
		/// than calling {@link #Next(Token)} instead.. 
		/// </summary>
		public virtual Token Next()
		{
            Token reusableToken = new Token();
			Token nextToken = Next(reusableToken);
			
			if (nextToken != null)
			{
				Payload p = nextToken.GetPayload();
				if (p != null)
				{
					nextToken.SetPayload((Payload) p.Clone());
				}
			}
			
			return nextToken;
		}
		
		/// <summary>Returns the next token in the stream, or null at EOS.
		/// When possible, the input Token should be used as the
		/// returned Token (this gives fastest tokenization
		/// performance), but this is not required and a new Token
		/// may be returned. Callers may re-use a single Token
		/// instance for successive calls to this method.
		/// <p>
		/// This implicitly defines a "contract" between 
		/// consumers (callers of this method) and 
		/// producers (implementations of this method 
		/// that are the source for tokens):
		/// <ul>
		/// <li>A consumer must fully consume the previously 
		/// returned Token before calling this method again.</li>
		/// <li>A producer must call {@link Token#Clear()}
		/// before setting the fields in it & returning it</li>
		/// </ul>
        /// Also, the producer must make no assumptions about a
        /// Token after it has been returned: the caller may
        /// arbitrarily change it.  If the producer needs to hold
        /// onto the token for subsequent calls, it must clone()
        /// it before storing it.
		/// Note that a {@link TokenFilter} is considered a consumer.
		/// </summary>
		/// <param name="reusableToken">a Token that may or may not be used to
        /// return; this parameter should never be null (the callee
        /// is not required to chedk for null before using it, but it is a
        /// good idea to assert that it is not null.)
		/// </param>
		/// <returns> next token in the stream or null if end-of-stream was hit
		/// </returns>
		public virtual Token Next(/* in */ Token reusableToken)
		{
            // We don't actually use inputToken, but still add the assert
			return Next();
		}
		
		/// <summary>Resets this stream to the beginning. This is an
		/// optional operation, so subclasses may or may not
		/// implement this method. Reset() is not needed for
		/// the standard indexing process. However, if the Tokens 
		/// of a TokenStream are intended to be consumed more than 
		/// once, it is necessary to implement reset().
   		/// once, it is necessary to implement reset().  Note that
		/// if your TokenStream caches tokens and feeds them back
		/// again after a reset, it is imperative that you
		/// clone the tokens when you store them away (on the
		/// first pass) as well as when you return them (on future
        /// passes after reset()).
		/// </summary>
		public virtual void  Reset()
		{
		}
		
		/// <summary>Releases resources associated with this stream. </summary>
		public virtual void  Close()
		{
		}
	}
}