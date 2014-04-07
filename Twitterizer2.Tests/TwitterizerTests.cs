using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Twitterizer;

namespace Twitterizer2.Tests
{
    [TestClass]
    public class TwitterizerTests
    {
        private OAuthTokens _oAuth;
        private List<string> _usersWithInt64UserIds;

        private void Setup()
        {
            _oAuth = new OAuthTokens
            {
                AccessToken = "1637195162-hHhJTPTdEXK2G0rVhBQG1M0XLuI7BKvSrSMgEAy",
                AccessTokenSecret = "s4XnJRN6eXUEwiU0HpgYnckl7xnKsr9anbh6ZorQwk",
                ConsumerKey = "TWNmvROzKsWGcaEVr3g",
                ConsumerSecret = "uc9PnfXxUisk9jcEZM171Wxk1ND6MkzA8kicrSESaNw"
            };

            //https://dev.twitter.com/blog/test-accounts-user-ids-greater-32-bits
            _usersWithInt64UserIds = new List<string>(new[] { "Overflow64" });
        }

        private const string ArtificialSearchResultPayload =
            "[     {      \"metadata\":  {        \"result_type\": \"recent\",        \"iso_language_code\": \"en\"      },      \"created_at\": \"Thu Apr 03 07:04:05 +0000 2014\",      \"id\": 451616109808980000,      \"id_str\": \"451616109808979968\",      \"text\": \"@MGreen_Aus Testing http://t.co/Bq4FNE7qqC\",      \"source\": \"\",      \"truncated\": false,      \"in_reply_to_status_id\": null,      \"in_reply_to_status_id_str\": null,      \"in_reply_to_user_id\": 1390744550,      \"in_reply_to_user_id_str\": \"1390744550\",      \"in_reply_to_screen_name\": \"MGreen_Aus\",      \"user\":  {        \"id\": 1390744550,        \"id_str\": \"1390744550\",        \"name\": \"Mark Green\",        \"screen_name\": \"MGreen_Aus\",        \"location\": \"Australia\",        \"description\": \"I write code, because it's fun.\\r\\nI ride bikes, because it's fun.\\r\\nI play games, because it's fun.\",        \"url\": null,        \"entities\":  {          \"description\":  {            \"urls\":  []          }        },        \"protected\": false,        \"followers_count\": 19,        \"friends_count\": 37,        \"listed_count\": 0,        \"created_at\": \"Mon Apr 29 23:58:06 +0000 2013\",        \"favourites_count\": 77,        \"utc_offset\": 39600,        \"time_zone\": \"Sydney\",        \"geo_enabled\": false,        \"verified\": false,        \"statuses_count\": 139,        \"lang\": \"en\",        \"contributors_enabled\": false,        \"is_translator\": false,        \"is_translation_enabled\": false,        \"profile_background_color\": \"1A1B1F\",        \"profile_background_image_url\": \"http://abs.twimg.com/images/themes/theme9/bg.gif\",        \"profile_background_image_url_https\": \"https://abs.twimg.com/images/themes/theme9/bg.gif\",        \"profile_background_tile\": false,        \"profile_image_url\": \"http://pbs.twimg.com/profile_images/3593333942/9af922dd84e6aad4c9de648ba56f069d_normal.jpeg\",        \"profile_image_url_https\": \"https://pbs.twimg.com/profile_images/3593333942/9af922dd84e6aad4c9de648ba56f069d_normal.jpeg\",        \"profile_link_color\": \"2FC2EF\",        \"profile_sidebar_border_color\": \"181A1E\",        \"profile_sidebar_fill_color\": \"252429\",        \"profile_text_color\": \"666666\",        \"profile_use_background_image\": true,        \"default_profile\": false,        \"default_profile_image\": false,        \"following\": null,        \"follow_request_sent\": null,        \"notifications\": null      },      \"geo\":  {        \"type\": \"Point\",        \"coordinates\":  [          -33.8499184,          150.8960654        ]      },      \"coordinates\":  {        \"type\": \"Point\",        \"coordinates\":  [          150.8960654,          -33.8499184        ]      },      \"place\":  {        \"id\": \"507cebbc685e4d02\",        \"url\": \"https://api.twitter.com/1.1/geo/id/507cebbc685e4d02.json\",        \"place_type\": \"city\",        \"name\": \"Fairfield\",        \"full_name\": \"Fairfield\",        \"country_code\": \"AU\",        \"country\": \"Australia\",        \"contained_within\":  [],        \"bounding_box\":  {          \"type\": \"Polygon\",          \"coordinates\":  [             [               [                150.81157845761,                -33.914184483792              ],               [                150.990520820674,                -33.914184483792              ],               [                150.990520820674,                -33.8206569570602              ],               [                150.81157845761,                -33.8206569570602              ]            ]          ]        },        \"attributes\":  {}      },      \"contributors\": null,      \"retweet_count\": 0,      \"favorite_count\": 0,      \"entities\":  {        \"hashtags\":  [],        \"symbols\":  [],        \"urls\":  [           {            \"url\": \"http://t.co/Bq4FNE7qqC\",            \"expanded_url\": \"http://testcreatesend.com/t/r-CD0C00DDF2F614F1\",            \"display_url\": \"testcreatesend.com/t/r-CD0C00DDF2…\",            \"indices\":  [              20,              42            ]          }        ],        \"user_mentions\":  [           {            \"screen_name\": \"MGreen_Aus\",            \"name\": \"Mark Green\",            \"id\": 1390744550,            \"id_str\": \"1390744550\",            \"indices\":  [              0,              11            ]          }        ]      },      \"favorited\": false,      \"retweeted\": false,      \"possibly_sensitive\": false,      \"lang\": \"en\"    }  ]";

        //[TestMethod]
        //public void CanDeserialize_64Bit_UserId()
        //{
        //    JsonConvert.DeserializeObject<TwitterSearchResultCollection>(
        //        );
        //}

        [TestMethod]
        public void TwitterUser_Lookup_Can_Handle_64Bit_Ids()
        {
            Setup();

            var response = TwitterUser.Lookup(_oAuth, new LookupUsersOptions
            {
                ScreenNames = new Collection<string>(_usersWithInt64UserIds)
            });

            Assert.IsTrue(response != null);
            Assert.IsTrue(response.ResponseObject != null);

            foreach (var user in response.ResponseObject)
                Assert.IsTrue(user.Id > Int32.MaxValue);
        }

        [TestMethod]
        public void TwitterUser_Lookup_Can_Read_String_Representation_Of_Ids()
        {
            Setup();

            var response = TwitterUser.Lookup(_oAuth, new LookupUsersOptions
            {
                ScreenNames = new Collection<string>(_usersWithInt64UserIds)
            });

            Assert.IsTrue(response != null);
            Assert.IsTrue(response.ResponseObject != null);

            foreach (var user in response.ResponseObject)
            {
                Assert.IsNotNull(user.StringId);
                Assert.AreNotEqual(user.StringId, String.Empty);
            }
        }

        [TestMethod]
        public void TwitterStatus_Can_Read_Geo_Coordinates()
        {
            var response = JsonConvert.DeserializeObject<TwitterSearchResultCollection>(ArtificialSearchResultPayload);
            var coords = response.First().Geo.Coordinates.FirstOrDefault();

            Assert.IsNotNull(coords);
            Assert.AreEqual(-33.8499184, coords.Latitude);
            Assert.AreEqual(150.8960654, coords.Longitude);
        }
    }
}
