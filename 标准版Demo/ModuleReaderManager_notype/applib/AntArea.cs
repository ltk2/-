using System;
using System.Collections.Generic;
using System.Text;

namespace AppAlgoLib
{

    public class TagDInfo
    {
        public int Antenna;
        public ushort CRC;
        public string EMDDataString;
        public string EPCString;
        public int Frequency;
        public ushort PC;
        public int Phase;
        public int ReadCount;
        public int ReadOffset;
        public int Rssi;

        public DateTime Time;

    }

    public class Ant_TagPar
    {
        public List<int> ant_times;//返回次数
        public List<int> ant_tagcounts;//计算总次数
        public List<int> ant_tagrssis;//计算平均crc
        public List<DateTime> ant_tagfirstime;//时间先后

    }

    public class Tag_AntPar
    {
        public List<int> tag_times;//返回次数
        public List<int> tag_antcounts;//计算总次数
        public List<int> tag_antrssis;//计算平均crc
        public List<DateTime> tag_antfirstime;//时间先后

    }

    public class CTA_Par
    {
        public int ant;
        public string tag;
        public int count;
        public int rssi;
        public int time;//返回多少次（一帧为一次）
        public DateTime dt;
    }

    public class AntArea
    {
        Dictionary<int, List<string>> dic_anttotags;//天线-标签字典
        Dictionary<string, List<int>> dic_tagtoants;//标签-天线字典
        List<TagDInfo> l_trds;//读到的原始数据

        Dictionary<string, bool> dic_tagtoantdif;//已区分的标签
        List<int> dic_anttotagdif;//需要降功率的天线
        List<string> l_filtertags;
        List<int> l_filterants;
        Dictionary<int, Ant_TagPar> diat;
        Dictionary<string, Tag_AntPar> dsta;
        List<CTA_Par> latp;
        string logmess = "";
        int antareamaxc;
        int firstcompare;
        int filtercount;
        int filterrssi;
        public int First_Cmp_Count = 0;
        public int First_Cmp_Rssi = 1;

        double pCount;//次数比例
        double pRssi;//信号比例
        bool isflask = false;
        public int AntMaxcount
        {
            get { return antareamaxc; }
        }
        public double PCount
        {
            set { pCount = value; }
        }

        public double PRssi
        {
            set { pRssi = value; }
        }

        public string Log
        {
            get { return logmess; }
        }

        public int FirstCompare
        {
            set { firstcompare = value; }
            get { return firstcompare; }
        }

        public int FilterCount
        {
            set { filtercount = value; }
            get { return filtercount; }
        }

        public int FilterRssi
        {
            set { filterrssi = value; }
            get { return filterrssi; }
        }

        public Dictionary<string, bool> Dic_diffTags
        {
            get { return dic_tagtoantdif; }
        }

        public List<int> L_diffAnts
        {
            get { return dic_anttotagdif; }
        }

        public void ClearFilterTags()
        {
            l_filtertags.Clear();
        }

        public void ClearFilterAnts()
        {
            l_filterants.Clear();
        }

        public void AddFilterTag(string tag)
        {

            if (!l_filtertags.Contains(tag))
                l_filtertags.Add(tag);
        }

        public void AddFilterAnt(int ant)
        {
            if (!l_filterants.Contains(ant))
                l_filterants.Add(ant);
        }


        public AntArea()
        {
            dic_anttotags = new Dictionary<int, List<string>>();
            dic_tagtoants = new Dictionary<string, List<int>>();
            l_trds = new List<TagDInfo>();
            latp = new List<CTA_Par>();
            diat = new Dictionary<int, Ant_TagPar>();
            dsta = new Dictionary<string, Tag_AntPar>();
            dic_tagtoantdif = new Dictionary<string, bool>();
            dic_anttotagdif = new List<int>();
            l_filtertags = new List<string>();
            l_filterants = new List<int>();

            antareamaxc = 0;

            firstcompare = 0;
            filtercount = 0;
            filterrssi = 0;

            pCount = 1.3;//次数比大于1.3认为是可区别的
            pRssi = 1.12;//信号比例
        }



        public void SetMaxantareaMaxcount(int maxcount)
        {
            logmess += "Max:" + maxcount + "\r\n";
            antareamaxc = maxcount;
        }
        public void Clear()
        {
            logmess = "";
            l_trds.Clear();
            latp.Clear();
            dic_anttotags.Clear();
            dic_tagtoants.Clear();
            diat.Clear();
            dsta.Clear();
            dic_tagtoantdif.Clear();
            dic_anttotagdif.Clear();
        }

        public void AddTagTo(TagDInfo read)
        {

            if (l_filtertags.Contains(read.EPCString))
                return;

            if (l_filterants.Contains(read.Antenna))
                return;

            if (filtercount == 0 && filterrssi == 0)
                l_trds.Add(read);
            else
            {
                bool isadd = true;
                if (filterrssi != 0 && read.Rssi <= filterrssi)
                    isadd = false;

                if (filtercount != 0 && read.ReadCount <= filtercount)
                    isadd = false;

                if (isadd)
                    l_trds.Add(read);
                else
                    logmess += "filter EPC:" + read.EPCString + " Ant:" + read.Antenna + " Count:" + read.ReadCount + " Rssi:" + read.Rssi + " Date:" + read.Time.ToString() + "\r\n";
            }
            //logmess += "EPC:" + read.EPCString +" Ant:"+read.Antenna+ " Count:" + read.ReadCount + " Rssi:" + read.Rssi + " Date:" + read.Time.ToString()+"\r\n";
            if (dic_anttotags.ContainsKey(read.Antenna))
            {
                if (!dic_anttotags[read.Antenna].Contains(read.EPCString))
                {

                    dic_anttotags[read.Antenna].Add(read.EPCString);
                    // logmess += "antkey:" + read.Antenna + " add tag:" + read.EPCString + "\r\n";
                }
            }
            else
            {
                dic_anttotags.Add(read.Antenna, new List<string>() { read.EPCString });
                // logmess += "add antkey:" + read.Antenna + " add tag:" + read.EPCString + "\r\n";
            }


            if (dic_tagtoants.ContainsKey(read.EPCString))
            {
                if (!dic_tagtoants[read.EPCString].Contains(read.Antenna))
                {
                    dic_tagtoants[read.EPCString].Add(read.Antenna);
                    // logmess += "tagkey:" + read.EPCString + " add ant:" + read.Antenna + "\r\n";
                }
            }
            else
            {
                dic_tagtoants.Add(read.EPCString, new List<int>() { read.Antenna });
                // logmess += "add tagkey:" + read.EPCString + " add ant:" + read.Antenna + "\r\n";
            }

            bool ishas = false;
            int index = 0;
            for (int i = 0; i < latp.Count; i++)
            {
                if (latp[i].ant == read.Antenna && latp[i].tag == read.EPCString)
                {
                    index = i;
                    ishas = true;
                    break;
                }
            }

            if (ishas)
            {
                latp[index].count += read.ReadCount;
                latp[index].rssi += read.Rssi;
                latp[index].time++;
            }
            else
            {
                CTA_Par ctap = new CTA_Par();
                ctap.tag = read.EPCString;
                ctap.ant = read.Antenna;
                ctap.dt = read.Time;
                ctap.rssi = read.Rssi;
                ctap.count = read.ReadCount;
                ctap.time = 1;
                latp.Add(ctap);
            }
        }

        private bool GetCountRssiby_ant_tag(int antkey, string tagkey, CTA_Par ctap)
        {
            bool ishas = false;
            int index = 0;
            for (int i = 0; i < latp.Count; i++)
            {
                if (latp[i].ant == antkey && latp[i].tag == tagkey)
                {
                    index = i;
                    ishas = true;
                    break;
                }
            }

            if (ishas)
            {
                ctap.ant = antkey;
                ctap.tag = tagkey;
                ctap.count = latp[index].count;
                ctap.rssi = latp[index].rssi;
                ctap.dt = latp[index].dt;
                ctap.time = latp[index].time;
                return true;
            }
            else
                return false;
        }

        private void SortAntsby_count_rssi_time(string key_tag, List<int> ants)
        {
            //盘点同一个标签，找出次数最多和RSSI最强的天线区域
            Tag_AntPar tap = new Tag_AntPar();
            tap.tag_antcounts = new List<int>();
            tap.tag_antrssis = new List<int>();
            tap.tag_antfirstime = new List<DateTime>();
            tap.tag_times = new List<int>();

            for (int i = 0; i < ants.Count; i++)
            {
                CTA_Par ctap = new CTA_Par();
                GetCountRssiby_ant_tag(ants[i], key_tag, ctap);
                tap.tag_antcounts.Add(ctap.count);
                tap.tag_antrssis.Add(ctap.rssi);
                tap.tag_antfirstime.Add(ctap.dt);
                tap.tag_times.Add(ctap.time);
            }


            //排序count rssi time
            for (int i = 0; i < ants.Count; i++)
            {
                for (int j = i + 1; j < ants.Count; j++)
                {
                    bool ischangge = false;

                    if (firstcompare == First_Cmp_Count)
                    {
                        if (tap.tag_antcounts[j] > tap.tag_antcounts[i])
                        {
                            ischangge = true;
                        }
                        else if (tap.tag_antcounts[j] == tap.tag_antcounts[i])
                        {
                            double f1 = tap.tag_antrssis[j] * 1.0 / tap.tag_times[j];
                            double f2 = tap.tag_antrssis[i] * 1.0 / tap.tag_times[i];
                            if (f1 > f2)
                            {
                                ischangge = true;
                            }
                            else if (f1 == f2)
                            {
                                if (tap.tag_antfirstime[j] < tap.tag_antfirstime[i])
                                {
                                    ischangge = true;
                                }
                            }
                        }
                    }
                    else
                    {

                        double f1 = tap.tag_antrssis[j] * 1.0 / tap.tag_times[j];
                        double f2 = tap.tag_antrssis[i] * 1.0 / tap.tag_times[i];
                        if (f1 > f2)
                        {
                            ischangge = true;
                        }
                        else if (f1 == f2)
                        {
                            if (tap.tag_antcounts[j] > tap.tag_antcounts[i])
                            {
                                ischangge = true;
                            }
                            else if (tap.tag_antcounts[j] == tap.tag_antcounts[i])
                            {
                                if (tap.tag_antfirstime[j] < tap.tag_antfirstime[i])
                                {
                                    ischangge = true;
                                }
                            }
                        }

                    }

                    if (ischangge)
                    {
                        int temp = ants[i];
                        ants[i] = ants[j];
                        ants[j] = temp;

                        temp = tap.tag_antcounts[i];
                        tap.tag_antcounts[i] = tap.tag_antcounts[j];
                        tap.tag_antcounts[j] = temp;

                        temp = tap.tag_antrssis[i];
                        tap.tag_antrssis[i] = tap.tag_antrssis[j];
                        tap.tag_antrssis[j] = temp;

                        temp = tap.tag_times[i];
                        tap.tag_times[i] = tap.tag_times[j];
                        tap.tag_times[j] = temp;

                        DateTime dtemp = tap.tag_antfirstime[i];
                        tap.tag_antfirstime[i] = tap.tag_antfirstime[j];
                        tap.tag_antfirstime[j] = dtemp;
                    }
                }

            }

            dsta.Add(key_tag, tap);
        }
        private void SortTagsby_count_rssi_time(int key_ant, List<string> tags)
        {
            //盘点同一个天线，次数最多和RSSI最强的标签
            Ant_TagPar atp = new Ant_TagPar();
            atp.ant_tagcounts = new List<int>();
            atp.ant_tagrssis = new List<int>();
            atp.ant_tagfirstime = new List<DateTime>();
            atp.ant_times = new List<int>();
            for (int i = 0; i < tags.Count; i++)
            {
                CTA_Par ctap = new CTA_Par();
                GetCountRssiby_ant_tag(key_ant, tags[i], ctap);
                atp.ant_tagcounts.Add(ctap.count);
                atp.ant_tagrssis.Add(ctap.rssi);
                atp.ant_tagfirstime.Add(ctap.dt);
                atp.ant_times.Add(ctap.time);
            }


            //排序count rssi time
            for (int i = 0; i < tags.Count; i++)
            {
                for (int j = i + 1; j < tags.Count; j++)
                {
                    bool ischangge = false;
                    if (firstcompare == First_Cmp_Count)
                    {
                        if (atp.ant_tagcounts[j] > atp.ant_tagcounts[i])
                        {
                            ischangge = true;
                        }
                        else if (atp.ant_tagcounts[j] == atp.ant_tagcounts[i])
                        {
                            double f1 = atp.ant_tagrssis[j] * 1.0 / atp.ant_times[j];
                            double f2 = atp.ant_tagrssis[i] * 1.0 / atp.ant_times[i];
                            if (f1 > f2)
                            {
                                ischangge = true;
                            }
                            else if (f1 == f2)
                            {
                                if (atp.ant_tagfirstime[j] < atp.ant_tagfirstime[i])
                                {
                                    ischangge = true;
                                }
                            }
                        }
                    }
                    else
                    {

                        double f1 = atp.ant_tagrssis[j] * 1.0 / atp.ant_times[j];
                        double f2 = atp.ant_tagrssis[i] * 1.0 / atp.ant_times[i];
                        if (f1 > f2)
                        {
                            ischangge = true;
                        }
                        else if (f1 == f2)
                        {
                            if (atp.ant_tagcounts[j] > atp.ant_tagcounts[i])
                            {
                                ischangge = true;
                            }
                            else if (atp.ant_tagcounts[j] == atp.ant_tagcounts[i])
                            {
                                if (atp.ant_tagfirstime[j] < atp.ant_tagfirstime[i])
                                {
                                    ischangge = true;
                                }
                            }
                        }

                    }

                    if (ischangge)
                    {
                        string strtemp = tags[i];
                        tags[i] = tags[j];
                        tags[j] = strtemp;

                        int temp = atp.ant_tagcounts[i];
                        atp.ant_tagcounts[i] = atp.ant_tagcounts[j];
                        atp.ant_tagcounts[j] = temp;

                        temp = atp.ant_tagrssis[i];
                        atp.ant_tagrssis[i] = atp.ant_tagrssis[j];
                        atp.ant_tagrssis[j] = temp;

                        temp = atp.ant_times[i];
                        atp.ant_times[i] = atp.ant_times[j];
                        atp.ant_times[j] = temp;

                        DateTime dtemp = atp.ant_tagfirstime[i];
                        atp.ant_tagfirstime[i] = atp.ant_tagfirstime[j];
                        atp.ant_tagfirstime[j] = dtemp;
                    }
                }

            }

            diat.Add(key_ant, atp);
        }

        private void printData_raw()
        {
            foreach (KeyValuePair<string, List<int>> kvp in dic_tagtoants)
            {
                logmess += " tagkey:" + kvp.Key + " sort= ";
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    if (kvp.Value.Count > 1)
                        logmess += " " + kvp.Value[i] + "(" + dsta[kvp.Key].tag_antcounts[i] + "," + dsta[kvp.Key].tag_antrssis[i] + ") ";
                    else
                        logmess += " " + kvp.Value[i] + " ";
                }
                logmess += "\r\n";

            }

            foreach (KeyValuePair<int, List<string>> kvp in dic_anttotags)
            {

                logmess += " antkey:" + kvp.Key + " sort= ";
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    if (kvp.Value.Count > 1)
                        logmess += " " + kvp.Value[i] + "(" + diat[kvp.Key].ant_tagcounts[i] + "," + diat[kvp.Key].ant_tagrssis[i] + ") ";
                    else
                        logmess += " " + kvp.Value[i] + " ";
                }
                logmess += "\r\n";
            }
        }

        private void printData_tagsort(List<string> tag_sort, Dictionary<string, List<int>> dic_tagtoants_sort)
        {
            for (int i = 0; i < tag_sort.Count; i++)
            {
                logmess += " tagkey:" + tag_sort[i] + " sort= ";
                for (int j = 0; j < dic_tagtoants_sort[tag_sort[i]].Count; j++)
                {
                    if (dic_tagtoants_sort[tag_sort[i]].Count > 1)
                        logmess += " " + dic_tagtoants_sort[tag_sort[i]][j] + "(" + dsta[tag_sort[i]].tag_antcounts[j] + "," + dsta[tag_sort[i]].tag_antrssis[j] + ") ";
                    else
                        logmess += " " + dic_tagtoants_sort[tag_sort[i]][j] + " ";
                }
                logmess += "\r\n";
            }
        }

        private void printData_antsort(List<int> ant_sort, Dictionary<int, List<string>> dic_anttotags_sort)
        {
            for (int i = 0; i < ant_sort.Count; i++)
            {
                logmess += " antkey:" + ant_sort[i] + " sort= ";
                for (int j = 0; j < dic_anttotags_sort[ant_sort[i]].Count; j++)
                {
                    if (dic_anttotags_sort[ant_sort[i]].Count > 1)
                        logmess += " " + dic_anttotags_sort[ant_sort[i]][j] + "(" + diat[ant_sort[i]].ant_tagcounts[j] + "," + diat[ant_sort[i]].ant_tagrssis[j] + ") ";
                    else
                        logmess += " " + dic_anttotags_sort[ant_sort[i]][j] + " ";
                }
                logmess += "\r\n";
            }
        }


        public void PreReAnysisData()
        {
            dic_tagtoantdif.Clear();
            dic_anttotagdif.Clear();
            diat.Clear();
            dsta.Clear();
            logmess = "";
        }

        /*
         * 比较1  true 大于，false 等于小于 
         */

        private bool Compare(CTA_Par c1, CTA_Par c2)
        {
            bool ismorethan = false;
            if (firstcompare == First_Cmp_Count)
            {
                if (c1.count > c2.count)
                    ismorethan = true;
                if (c1.count == c2.count)
                {
                    double p1 = c1.rssi * 1.0 / c1.time;
                    double p2 = c2.rssi * 1.0 / c2.time;
                    if (p1 > p2)
                        ismorethan = true;
                    else if (p1 == p2)
                    {
                        if (c1.dt < c2.dt)
                            ismorethan = true;
                    }
                }
            }
            else
            {

                double p1 = c1.rssi * 1.0 / c1.time;
                double p2 = c2.rssi * 1.0 / c2.time;
                if (p1 > p2)
                    ismorethan = true;
                else if (p1 == p2)
                {
                    if (c1.count > c2.count)
                        ismorethan = true;
                    else if (c1.count == c2.count)
                    {
                        if (c1.dt < c2.dt)
                            ismorethan = true;
                    }
                }

            }

            return ismorethan;
        }

        private bool isDiff(int antkey1, string tagkey1, int antkey2, string tagkey2, string msg)
        {
            CTA_Par ctap = new CTA_Par();
            bool bl = GetCountRssiby_ant_tag(antkey1, tagkey1, ctap);
            int count_1 = ctap.count;
            int rssi_1 = ctap.rssi;
            int time_1 = ctap.time;

            GetCountRssiby_ant_tag(antkey2, tagkey2, ctap);
            int count_2 = ctap.count;
            int rssi_2 = ctap.rssi;
            int time_2 = ctap.time;

            double f1 = count_1 * 1.0 / count_2;
            double f2 = 1 / (rssi_1 / time_1 * 1.0 / (rssi_2 / time_2));

            logmess += msg + " f1:" + String.Format("{0:F}", f1) + " f2:" + String.Format("{0:F}", f2) + " \r\n";
            //判断是否明显差异
            if (f1 > this.pCount && f2 > this.pRssi)
                return true;
            else
                return false;

        }

        private void DeleAntandTag(int antkey, string tagkey, string msg, Dictionary<string, List<int>> dic_tagtoants_sort, Dictionary<int, List<string>> dic_anttotags_sort)
        {
            logmess += msg + " ";
            dic_tagtoants_sort[tagkey].Remove(antkey);

            dic_anttotags_sort[antkey].Remove(tagkey);
            logmess += " tagkey:" + tagkey + " antkey:" + antkey + "\r\n";
        }

        /*
         * 当找到标签最终所属的天线区域时候处理
         */
        private void HanldeTagofTheLastAnt(int antkeylast, string tagkey, Dictionary<string, List<int>> dic_tagtoants_sort, Dictionary<int, List<string>> dic_anttotags_sort)
        {
            string ftagkey = dic_anttotags_sort[antkeylast][0];

            string[] rtagkeys = dic_anttotags_sort[antkeylast].ToArray();
            int fc = dic_anttotags_sort[antkeylast].Count - 1;

            if (fc <= antareamaxc)
            {
                if (!dic_tagtoantdif.ContainsKey(tagkey))
                {

                    dic_tagtoantdif.Add(tagkey, true);
                    logmess += "diffto add only 1  ant tagkey:" + tagkey + "\r\n";

                }
            }

            for (int k = fc; k >= 0; k--)
            {
                string removetagkey = rtagkeys[k];

                if (removetagkey != tagkey)
                {

                    if (dic_tagtoants_sort[removetagkey].Count > 1)
                    {
                        //若标签存在对应多个天线区域需要处理

                        if (antareamaxc > 0 && dic_anttotags_sort[antkeylast].Count > antareamaxc)
                        {

                            string msg = "diffto 2 A<T>  tagkey:" + removetagkey + " lastantkey:" + antkeylast;

                            bool isdiff = isDiff(antkeylast, ftagkey, antkeylast, removetagkey, msg);

                            //判断是否明显差异
                            if (isdiff)
                            {
                                //若改该天线区域有所限制标签个数又已超出则直接去掉差别大的该标签
                                DeleAntandTag(antkeylast, removetagkey, "remove 2 A<T>", dic_tagtoants_sort, dic_anttotags_sort);

                            }
                        }
                        else
                        {
                            //若没有个数限制或者未超出限制个数

                            if (dic_tagtoants_sort[removetagkey][0] == antkeylast)
                            {
                                //若该天线为标签第一个天线则，删掉标签差别大的其它天线

                                while (dic_tagtoants_sort[removetagkey].Count > 1)
                                {
                                    int del = dic_tagtoants_sort[removetagkey].Count - 1;
                                    int delant = dic_tagtoants_sort[removetagkey][del];

                                    string msg = "diffto  3 T<A>  tagkey:" + removetagkey + " lastantkey:" + antkeylast;

                                    bool isdiff = isDiff(antkeylast, ftagkey, delant, removetagkey, msg);

                                    //判断是否明显差异
                                    if (isdiff)
                                    {
                                        DeleAntandTag(delant, removetagkey, "remove 3 T<A>", dic_tagtoants_sort, dic_anttotags_sort);
                                    }

                                    break;
                                }
                            }
                            else
                            {
                                string msg = "diffto 4 T<A> tagkey:" + removetagkey + " lastantkey:" + antkeylast;

                                bool isdiff = isDiff(antkeylast, ftagkey, antkeylast, removetagkey, msg);

                                //判断是否明显差异
                                if (isdiff)
                                {
                                    DeleAntandTag(antkeylast, removetagkey, "remove 4 T<A>", dic_tagtoants_sort, dic_anttotags_sort);
                                }
                            }
                        }
                    }
                }
            }
        }

        public Dictionary<int, List<string>> AnysisData()
        {
            int step = 0;

            //备份原始字典，使用副本进行排序删减
            Dictionary<int, List<string>> dic_anttotags_sort = new Dictionary<int, List<string>>();//天线-标签字典
            foreach (KeyValuePair<int, List<string>> kvp in dic_anttotags)
            {
                List<string> ls = new List<string>();
                ls.AddRange(kvp.Value);
                dic_anttotags_sort.Add(kvp.Key, ls);
            }
            Dictionary<string, List<int>> dic_tagtoants_sort = new Dictionary<string, List<int>>();//标签-天线字典
            foreach (KeyValuePair<string, List<int>> kvp in dic_tagtoants)
            {
                List<int> ls = new List<int>();
                ls.AddRange(kvp.Value);
                dic_tagtoants_sort.Add(kvp.Key, ls);
            }

            try
            {

                //每个标签只在一个天线区域，如出现多个天线区域，则把这些天线区域按次数多，信号强优先排序
                List<string> tag_sort = new List<string>();
                foreach (KeyValuePair<string, List<int>> kvp in dic_tagtoants_sort)
                {
                    // if (kvp.Value.Count > 1)
                    SortAntsby_count_rssi_time(kvp.Key, kvp.Value);
                    tag_sort.Add(kvp.Key);

                }

                step = 1;
                /**
                *   再排序tag，按最次数多，信号强优先排序tag
                * */

                for (int i = 0; i < tag_sort.Count; i++)
                {
                    for (int j = i + 1; j < tag_sort.Count; j++)
                    {
                        bool ischange = false;

                        if (dic_tagtoants_sort[tag_sort[i]].Count > 1)
                        {
                            if (dic_tagtoants_sort[tag_sort[j]].Count == 1)
                            {
                                ischange = true;
                            }
                            else if (dic_tagtoants_sort[tag_sort[j]].Count > 1)
                            {
                                /**
                                 * 标签具有多个天线，但天线只有唯一的标签排在前面判断
                                 */
                                bool isonlytagforoneant_i = false;
                                bool isonlytagforoneant_j = false;

                                foreach (KeyValuePair<int, List<string>> kvp in dic_anttotags_sort)
                                {
                                    if (kvp.Value.Count == 1 && kvp.Value.Contains(tag_sort[i]))
                                        isonlytagforoneant_i = true;
                                    if (kvp.Value.Count == 1 && kvp.Value.Contains(tag_sort[j]))
                                        isonlytagforoneant_j = true;

                                }

                                if (isonlytagforoneant_i == false && isonlytagforoneant_j == false)
                                {
                                    step = 11;
                                    //次数，rssi最强 排序
                                    CTA_Par ctap1 = new CTA_Par();
                                    GetCountRssiby_ant_tag(dic_tagtoants_sort[tag_sort[i]][0], tag_sort[i], ctap1);

                                    CTA_Par ctap2 = new CTA_Par();
                                    GetCountRssiby_ant_tag(dic_tagtoants_sort[tag_sort[j]][0], tag_sort[j], ctap2);

                                    ischange = Compare(ctap2, ctap1);
                                }
                                else
                                {
                                    if (isonlytagforoneant_j)
                                        ischange = true;
                                }
                            }
                        }

                        if (ischange)
                        {
                            string temp = tag_sort[i];
                            tag_sort[i] = tag_sort[j];
                            tag_sort[j] = temp;
                        }

                    }
                }
                step = 2;
                printData_tagsort(tag_sort, dic_tagtoants_sort);
                step = 3;


                //同一个天线区域，对标签进行排序，读到最好的排在最前
                List<int> ant_sort = new List<int>(); //
                foreach (KeyValuePair<int, List<string>> kvp in dic_anttotags_sort)
                {
                    // if (kvp.Value.Count > 1)
                    SortTagsby_count_rssi_time(kvp.Key, kvp.Value);
                    ant_sort.Add(kvp.Key);

                }
                step = 4;
                for (int i = 0; i < ant_sort.Count; i++)
                {
                    for (int j = i + 1; j < ant_sort.Count; j++)
                    {
                        bool ischange = false;


                        if (dic_anttotags_sort[ant_sort[i]].Count > 1)
                        {
                            if (dic_anttotags_sort[ant_sort[j]].Count == 1)
                            {
                                ischange = true;
                            }
                            else if (dic_anttotags_sort[ant_sort[j]].Count > 1)
                            {
                                //次数，rssi最强 排序
                                CTA_Par ctap1 = new CTA_Par();
                                GetCountRssiby_ant_tag(ant_sort[i], dic_anttotags_sort[ant_sort[i]][0], ctap1);

                                CTA_Par ctap2 = new CTA_Par();
                                GetCountRssiby_ant_tag(ant_sort[j], dic_anttotags_sort[ant_sort[j]][0], ctap2);

                                ischange = Compare(ctap2, ctap1);
                            }
                        }


                        if (ischange)
                        {
                            int temp = ant_sort[i];
                            ant_sort[i] = ant_sort[j];
                            ant_sort[j] = temp;
                        }
                    }
                }
                printData_antsort(ant_sort, dic_anttotags_sort);

                step = 5;

                for (int i = 0; i < tag_sort.Count; i++)
                {
                    string tagkey = tag_sort[i];

                    if (dic_tagtoants_sort[tagkey].Count > 1)
                    {

                        //比较 
                        int antkey = dic_tagtoants_sort[tagkey][0];

                        while (dic_tagtoants_sort[tagkey].Count > 1)
                        {
                            int removeantkey = dic_tagtoants_sort[tagkey][dic_tagtoants_sort[tagkey].Count - 1];

                            string msg = "diffto 1 T<A> tagkey:" + tagkey;

                            bool isdiff = isDiff(antkey, tagkey, removeantkey, tagkey, msg);

                            //判断是否明显差异
                            if (isdiff)
                            {

                                DeleAntandTag(removeantkey, tagkey, "remove 1 T<A>", dic_tagtoants_sort, dic_anttotags_sort);
                            }
                            else
                                break;

                        }
                    }

                    int antkeylast = dic_tagtoants_sort[tagkey][0];

                    if (dic_tagtoants_sort[tagkey].Count == 1)
                    {

                        HanldeTagofTheLastAnt(antkeylast, tagkey, dic_tagtoants_sort, dic_anttotags_sort);
                    }
                    else
                    {
                        dic_tagtoantdif.Add(tagkey, false);
                        for (int j = 0; j < dic_tagtoants_sort[tagkey].Count; j++)
                            if (!dic_anttotagdif.Contains(dic_tagtoants_sort[tagkey][j]))
                            dic_anttotagdif.Add(dic_tagtoants_sort[tagkey][j]);
                    }

                }

                
                 
                {
                    step = 7;
                    foreach (KeyValuePair<int, List<string>> kvp in dic_anttotags_sort)
                    {

                        //强制删掉超出归属天线的管理个数的只有唯一该天线的标签
                        while ((antareamaxc > 0) && (dic_anttotags_sort[kvp.Key].Count > antareamaxc))
                        {
                            string overmaxtag = dic_anttotags_sort[kvp.Key][0];
                            string overmaxtag2 = dic_anttotags_sort[kvp.Key][dic_anttotags_sort[kvp.Key].Count - 1];

                            //优先比较唯一天线 

                            if (dic_tagtoants_sort[overmaxtag].Count > 1 && dic_tagtoants_sort[overmaxtag2].Count == 1)
                            {
                                logmess += " remove antkeylast:" + kvp.Key + " overmaxtag:" + overmaxtag + "\r\n";
                                dic_anttotags_sort[kvp.Key].Remove(overmaxtag);
                                continue;
                            }
                            if (dic_tagtoants_sort[overmaxtag2].Count > 1 && dic_tagtoants_sort[overmaxtag].Count == 1)
                            {
                                logmess += " remove antkeylast:" + kvp.Key + " overmaxtag2:" + overmaxtag2 + "\r\n";
                                dic_anttotags_sort[kvp.Key].Remove(overmaxtag2);
                                continue;
                            }

                            string msg = "diffto 5 A<T> antkey:" + kvp.Key + ",lasttag:" + overmaxtag2;
                            bool isdif = isDiff(kvp.Key, overmaxtag, kvp.Key, overmaxtag2, msg);

                            if (isdif)
                                DeleAntandTag(kvp.Key, overmaxtag2, "remove 5 f A<T>", dic_tagtoants_sort, dic_anttotags_sort);
                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace + ex.Message + " " + step);
            }

            return dic_anttotags_sort;
        }
    }
}
