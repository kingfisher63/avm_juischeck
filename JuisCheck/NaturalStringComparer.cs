/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;

namespace JuisCheck
{
	public class NaturalStringComparer : Comparer<string>
	{
		private enum TokenType
		{
			Nothing,
			Numerical,
			String,
			End
		}

		private sealed class StringParser
		{
			private	string		mSource;
			private	int			mLength;
			private	int			mCurrentIndex;
			private char		mCurrentChar;

			internal void Init( string str )
			{
				mSource       = str ?? string.Empty;
				mLength       = mSource.Length;
				mCurrentIndex = -1;

				TokenType      = TokenType.Nothing;
				NumericalValue = 0;
				StringValue    = null;

				NextChar();
			}

			private void NextChar()
			{
				mCurrentIndex++;
				mCurrentChar = mCurrentIndex >= mLength ? '\0' : mSource[mCurrentIndex];
			}

			internal void NextToken()
			{
				NumericalValue = 0;
				StringValue    = null;

				do {
					// End of string
					if (mCurrentChar == '\0') {
						TokenType = TokenType.End;
						return;
					}

					// Numerical token
					if (char.IsDigit(mCurrentChar)) {
						int startIndex = mCurrentIndex;

						do {
							NextChar();
						}
						while (char.IsDigit(mCurrentChar));

						StringValue = mSource.Substring(startIndex, mCurrentIndex - startIndex);
						TokenType   = TokenType.String;

						if (decimal.TryParse(StringValue, out decimal value)) {
							NumericalValue = value;
							TokenType      = TokenType.Numerical;
						}

						return;
					}

					// String token
					if (char.IsLetter(mCurrentChar)) {
						int startIndex = mCurrentIndex;

						do {
							NextChar();
						}
						while (char.IsLetter(mCurrentChar));

						StringValue = mSource.Substring(startIndex, mCurrentIndex - startIndex);
						TokenType   = TokenType.String;
						return;
					}

					NextChar();
				}
				while (true);
			}

			internal decimal	NumericalValue	{ get; private set; } = 0;
			internal string		StringValue		{ get; private set; } = null;
			internal TokenType	TokenType		{ get; private set; } = TokenType.Nothing;
		}

		private readonly StringParser		mParser1;
		private readonly StringParser		mParser2;
		private readonly StringComparison	mStringComparison;

		public NaturalStringComparer( StringComparison stringComparison = StringComparison.CurrentCulture )
		{
			mParser1          = new StringParser();
			mParser2          = new StringParser();
			mStringComparison = stringComparison;
		}

		public override int Compare( string str1, string str2 )
		{
			int result;

			mParser1.Init(str1);
			mParser2.Init(str2);

			do { 
				mParser1.NextToken();
				mParser2.NextToken();

				if (mParser1.TokenType == TokenType.Numerical && mParser2.TokenType == TokenType.Numerical) {
					result = decimal.Compare(mParser1.NumericalValue, mParser2.NumericalValue);
					if (result != 0) {
						return result;
					}
				}

				result = string.Compare(mParser1.StringValue, mParser2.StringValue, mStringComparison);
				if (result != 0) {
					return result;
				}
			}
			while (mParser1.TokenType != TokenType.End || mParser2.TokenType != TokenType.End);
			
			return 0;
		}
	}
}
