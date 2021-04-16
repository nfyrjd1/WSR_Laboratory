using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WSR_Laboratory_Mobile.Model
{
    public class News
    {
        public int Id { get; set; }
        [JsonProperty("Заголовок")]
        public string Title { get; set; }

        [JsonProperty("Дата")]
        public string Date { get; set; }

        [JsonProperty("Текст")]
        public string Text { get; set; }
    }
}
