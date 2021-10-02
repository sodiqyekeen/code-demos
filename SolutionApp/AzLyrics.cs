using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolutionApp
{

    public class Lyrics
    {
        public string Link { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
    }
    public class AzLyrics
    {
        private const string rowPattern = @"<td\s*.*>(((\s*.*))\s*)<\/td>";
        private const string linkPattern = @"((\w+:\/\/)[-a-zA-Z0-9:@;?&=\/%\+\.\*!'\(\),\$_\{\}\^~\[\]`#|]+)";
        private const string textPattern = @"<b>([^</>]*)<\/b>";

        private string _resultString;

        public async Task<string> GetLyricsContent(string link)
        {
            var client = new HttpClient();
            string response = await client.GetStringAsync(link);
            string lyrics = GetText(response.Between("<div>", "</div>")).Trim();
            return lyrics;
        }


        public async Task<string> Search(string artist, string song)
        {
            string url = $"https://search.azlyrics.com/search.php?q={artist}+{song}&w=songs&p=1";
            HttpClient client = new HttpClient();
            _resultString = await client.GetStringAsync(url);
            if (!_resultString.Contains("Song results:"))
                return null;
            var matchedRows = Regex.Matches(_resultString, rowPattern);

            string result = null;
            foreach (Match row in matchedRows)
            {
                var rowObject = GetRowObject(row.Value);
                if (artist.Equals(rowObject.Artist, StringComparison.OrdinalIgnoreCase) && song.Equals(rowObject.Title, StringComparison.OrdinalIgnoreCase))
                {
                    result = await GetLyricsContent(rowObject.Link);
                    break;
                }
            }
            return result;
        }

        private static Lyrics GetRowObject(string rowString)
        {
            string link = GetText(Regex.Match(rowString, linkPattern).Value);
            var texts = Regex.Matches(rowString, textPattern);
            string artist = GetText(texts[1].Value);
            string title = GetText(texts[0].Value).Trim();
            return new Lyrics
            {
                Link = link,
                Artist = artist,
                Title = title
            };
        }

        public static string GetText(string value)
        {
            value = Regex.Replace(value, "<.+?>", "");
            return value.Trim(new char[] { '"', '\'' });
        }

        private int GetRecordCount()
        {
            var matchHeaderTag = Regex.Match(_resultString, @"<small>((.*))?<\/small>");
            if (matchHeaderTag.Success)
            {
                string tag = matchHeaderTag.Value;
                var count = tag.Between("of", "found");
                if (!string.IsNullOrEmpty(count))
                    return int.Parse(count.Trim());
            }
            return -1;
        }
    }

}
