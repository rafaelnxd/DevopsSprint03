﻿namespace Challenge_Sprint03.Models.DTOs
{
    public class HabitoUpdateDTO
    {
        public int HabitoId { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public int FrequenciaIdeal { get; set; }
    }
}
