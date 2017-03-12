﻿// Copyright (c) 2016-2017 meil
//
// This file is part of Twichirp.
// 
// Twichirp is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Twichirp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with Twichirp.  If not, see <http://www.gnu.org/licenses/>.
using SRegex = System.Text.RegularExpressions.Regex;

namespace Twichirp.Core.TweetCount {
    public class Regex {

        private static readonly string LatinAccentsChars = "\\u00c0-\\u00d6\\u00d8-\\u00f6\\u00f8-\\u00ff\\u0100-\\u024f\\u0253\\u0254\\u0256\\u0257" +
            "\\u0259\\u025b\\u0263\\u0268\\u026f\\u0272\\u0289\\u028b\\u02bb\\u0300-\\u036f\\u1e00-\\u1eff";

        private static readonly string UrlValidChars = $"a-zA-Z0-9{LatinAccentsChars}";
        private static readonly string UrlValidPreceedingChars = "(?:[^A-Z0-9@＠$#＃\u202A-\u202E]|^)";
        private static readonly string UrlValidUnicodeChars = @"[.[^\p{P}\s\p{Z}\p{IsGeneralPunctuation}]]";
        private static readonly string UrlValidGeneralPathChars = $"[a-z\\p{{IsCyrillic}}0-9!\\*';:=\\+,.\\$/%#\\[\\]\\-_~\\|&@{LatinAccentsChars}]";

        private static readonly string UrlBalancedParens = $"\\((?:{UrlValidGeneralPathChars}+|(?:{UrlValidGeneralPathChars}*\\({UrlValidGeneralPathChars}+\\){UrlValidGeneralPathChars}*))\\)";

        private static readonly string UrlValidPathEndingChars = $"[a-z\\p{{IsCyrillic}}0-9=_#/\\-\\+{LatinAccentsChars}]|(?:{UrlBalancedParens})";
        private static readonly string UrlValidUrlQueryChars = "[a-z0-9!?\\*'\\(\\);:&=\\+\\$/%#\\[\\]\\-_\\.,~\\|@]";
        private static readonly string UrlValidUrlQueryEndingChars = "[a-z0-9_&=#/]";

        private static readonly string UrlPunycode = "(?:xn--[0-9a-z]+)";

        private static readonly string SpecialUrlValidCctld = "(?:(?:co|tv)(?=[^a-zA-Z0-9@]|$))";

        private static readonly string UrlVaildProtocol = "https?://";
        private static readonly string UrlValidSubdomain = $"(?>(?:[{UrlValidChars}][{UrlValidChars}\\-_]*)?[{UrlValidChars}]\\.)";
        private static readonly string UrlValidDomainName = $"(?:(?:[{UrlValidChars}][{UrlValidChars}\\-]*)?[{UrlValidChars}]\\.)";
        private static readonly string UrlValidGtld = $"(?:(?:{string.Join("|", TldLists.Gtldls)})(?=[^a-zA-Z0-9@]|$))";
        private static readonly string UrlValidCctld = $"(?:(?:{string.Join("|", TldLists.Ctlds)})(?=[^a-zA-Z0-9@]|$))";
        private static readonly string UrlValidDomain =
            $"(?:{UrlValidSubdomain}+{UrlValidDomainName}(?:{UrlValidGtld}|{UrlValidCctld}|{UrlPunycode}))" +
            $"|(?:{UrlValidDomainName}(?:{UrlValidGtld}|{UrlPunycode}|{SpecialUrlValidCctld}))" +
            $"|(?:(?<=https?://)(?:(?:{UrlValidDomainName}{UrlValidCctld})|(?:{UrlValidUnicodeChars}+\\.(?:{UrlValidGtld}|{UrlValidCctld}))))" +
            $"|(?:{UrlValidDomainName}{UrlValidCctld}(?=/))";
        private static readonly string UrlValidPortNumber = "[0-9]+";
        private static readonly string UrlValidPath = $"(?:(?:{UrlValidGeneralPathChars}*(?:{UrlBalancedParens}{UrlValidGeneralPathChars}*)*{UrlValidPathEndingChars})|(?:@{UrlValidGeneralPathChars}+/))";

        private static readonly string ValidUrlPattern = $"(({UrlValidPreceedingChars})(({UrlVaildProtocol})?({UrlValidDomain})(?::({UrlValidPortNumber}))?" +
            $"(/{UrlValidPath}*)?(\\?{UrlValidUrlQueryChars}*{UrlValidUrlQueryEndingChars})?))";

        public static SRegex ValidUrl { get; } = new SRegex(ValidUrlPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }

    internal class TldLists {
        internal static readonly string[] Gtldls = {"삼성","닷컴","닷넷","香格里拉","餐厅","食品", "飞利浦","電訊盈科", "集团", "购物", "谷歌", "诺基亚", "联通", "网络", "网站", "网店",
            "网址","组织机构","移动","珠宝","点看","游戏","淡马锡","机构","書籍","时尚", "新闻", "政府", "政务", "手表", "手机", "我爱你", "慈善", "微博","广东","工行", "家電","娱乐",
            "大拿","在线", "嘉里大酒店","嘉里","商标","商店", "商城","公益","公司","八卦","健康","信息", "佛山", "企业", "中文网","中信", "世界","ポイント", "ファッション","セール",
            "ストア","コム","グーグル","クラウド", "みんな","คอม", "संगठन", "नेट","कॉम", "همراه","موقع", "موبايلي", "كوم", "شبكة", "بيتك", "بازار","العليان", "ارامكو", "ابوظبي",
            "קום","сайт","рус","орг", "онлайн","москва","ком","дети", "zuerich", "zone", "zippo", "zip", "zero", "zara", "zappos", "yun","youtube",
            "you","yokohama","yoga","yodobashi","yandex","yamaxun","yahoo","yachts", "xyz","xxx","xperia","xin","xihuan","xfinity", "xerox", "xbox", "wtf","wtc", "world", "works",
            "work", "woodside","wolterskluwer", "wme","wine", "windows","win", "williamhill", "wiki", "wien", "whoswho", "weir", "weibo", "wedding",  "wed", "website", "weber", "webcam",
            "weatherchannel", "weather","watches","watch", "warman", "wanggou", "wang", "walter", "wales", "vuelos", "voyage", "voto", "voting", "vote", "volkswagen", "vodka",
            "vlaanderen", "viva","vistaprint", "vista", "vision", "virgin", "vip","vin", "villas", "viking", "vig", "video", "viajes", "vet", "versicherung", "vermögensberatung",
            "vermögensberater","verisign", "ventures","vegas", "vana", "vacations", "ups", "uol", "uno", "university","unicom", "ubs", "tvs",  "tushu", "tunes", "tui", "tube",  "trv",
            "trust","travelersinsurance","travelers","travelchannel","travel","training", "trading", "trade", "toys", "toyota",  "town", "tours", "total",  "toshiba", "toray", "top",
            "tools","tokyo",  "today","tmall","tirol","tires", "tips","tiffany","tienda", "tickets","theatre", "theater", "thd","teva", "tennis", "temasek", "telefonica", "telecity",
            "tel","technology", "tech", "team", "tdk", "tci", "taxi",  "tax", "tattoo", "tatar",  "tatamotors", "taobao",  "talk", "taipei",  "tab", "systems", "symantec", "sydney",
            "swiss", "swatch", "suzuki","surgery", "surf", "support", "supply", "supplies", "sucks", "style", "study", "studio", "stream", "store", "storage", "stockholm", "stcgroup",
            "stc","statoil","statefarm","statebank", "starhub", "star", "stada",  "srl", "spreadbetting", "spot",  "spiegel", "space", "soy", "sony", "song", "solutions", "solar",
            "sohu","software", "softbank", "social", "soccer", "sncf","smile", "skype", "sky", "skin", "ski", "site", "singles", "sina",  "silk", "shriram", "show", "shouji", "shopping",
            "shop","shoes", "shiksha", "shia",  "shell", "shaw",  "sharp",  "shangrila",  "sfr", "sexy",  "sex", "sew",  "seven", "services", "sener", "select", "seek", "security", "seat",
            "scot", "scor", "science", "schwarz", "schule", "school",  "scholarships", "schmidt", "schaeffler",  "scb", "sca", "sbs", "sbi", "saxo", "save", "sas", "sarl", "sapo",
            "sap","sanofi", "sandvikcoromant", "sandvik", "samsung", "salon",  "sale","sakura",  "safety", "safe", "saarland","ryukyu", "rwe",  "run", "ruhr", "rsvp", "room", "rodeo",
            "rocks", "rocher", "rip", "rio","ricoh", "richardli","rich", "rexroth",  "reviews", "review", "restaurant", "rest",  "republican", "report", "repair", "rentals", "rent",
            "ren", "reit","reisen", "reise","rehab", "redumbrella", "redstone", "red","recipes",  "realty", "realtor", "realestate", "read", "racing",  "quest","quebec", "qpon",
            "pwc","pub", "protection","property","properties", "promo", "progressive","prof", "productions","prod", "pro", "prime", "press", "praxi", "post", "porn", "politie","poker",
            "pohl",  "pnc","plus", "plumbing","playstation", "play","place",  "pizza","pioneer", "pink", "ping",  "pin", "pid",  "pictures","pictet", "pics", "piaget", "physio", "photos",
            "photography","photo", "philips","pharmacy", "pet","pccw", "passagens","party", "parts", "partners","pars","paris", "panerai","pamperedchef", "page","ovh", "ott", "otsuka",
            "osaka", "origins", "orientexpress","organic","org", "orange", "oracle", "ooo", "online", "onl", "ong", "one", "omega", "ollo", "olayangroup","olayan","okinawa","office",
            "obi","nyc", "ntt", "nrw","nra", "nowtv","nowruz", "now", "norton", "northwesternmutual", "nokia","nissay", "nissan",  "ninja", "nikon", "nico", "nhk",  "ngo", "nfl",
            "nexus", "nextdirect", "next", "news", "new", "neustar", "network", "netflix", "netbank", "net", "nec", "navy", "natura",  "name", "nagoya", "nadex", "mutuelle", "mutual",
            "museum", "mtr", "mtpc","mtn","movistar", "movie", "mov", "motorcycles",  "moscow",  "mortgage", "mormon", "montblanc", "money", "monash",  "mom", "moi", "moe",  "moda",
            "mobily", "mobi", "mma", "mls", "mlb","mitsubishi", "mit",  "mini",  "mil", "microsoft", "miami",  "metlife",  "meo", "menu",  "men", "memorial", "meme",  "melbourne",
            "meet","media", "med", "mba", "mattel",  "marriott", "markets",  "marketing", "market", "mango", "management",  "man",  "makeup", "maison", "maif", "madrid","luxury",
            "luxe", "lupin", "ltda", "ltd", "love", "lotto","lotte", "london", "lol",  "locus", "locker", "loans", "loan", "lixil",  "living", "live", "lipsy","link", "linde",
            "lincoln", "limo", "limited", "like", "lighting", "lifestyle", "lifeinsurance", "life", "lidl", "liaison", "lgbt", "lexus", "lego",  "legal", "leclerc", "lease", "lds",
            "lawyer", "law", "latrobe", "lat","lasalle",  "lanxess", "landrover", "land", "lancaster","lamer", "lamborghini", "lacaixa", "kyoto", "kuokgroup","kred", "krd", "kpn", "kpmg",
            "kosher", "komatsu", "koeln", "kiwi","kitchen", "kindle", "kinder", "kim",  "kia", "kfh", "kerryproperties",  "kerrylogistics",  "kerryhotels", "kddi",  "kaufen", "juegos",
            "jprs", "jpmorgan", "joy",  "jot", "joburg",  "jobs", "jnj", "jmp", "jll",  "jlc", "jewelry", "jetzt", "jcp",  "jcb",  "java","jaguar", "iwc",  "itv", "itau", "istanbul",
            "ist", "ismaili", "iselect", "irish", "ipiranga", "investments",  "international", "int", "insure",  "insurance", "institute",  "ink", "ing", "info", "infiniti", "industries",
            "immobilien", "immo", "imdb", "imamat", "ikano", "iinet",  "ifm", "icu",  "ice", "icbc", "ibm",  "hyundai", "htc",  "hsbc", "how",  "house", "hotmail",  "hoteles", "hosting",
            "host", "horse", "honda", "homes", "homedepot", "holiday", "holdings", "hockey", "hkt", "hiv", "hitachi", "hisamitsu", "hiphop", "hgtv", "hermes", "here",  "helsinki", "help",
            "healthcare","health", "hdfcbank", "haus", "hangout", "hamburg", "guru",  "guitars", "guide", "guge", "gucci","guardian", "group", "gripe",  "green", "gratis", "graphics",
            "grainger", "gov", "got", "gop", "google", "goog",  "goodyear", "goo", "golf",  "goldpoint","gold", "godaddy", "gmx", "gmo", "gmbh", "gmail",  "globo", "global",  "gle",
            "glass",  "giving", "gives", "gifts", "gift", "ggee", "genting", "gent", "gea", "gdn", "gbiz", "garden", "games", "game", "gallup", "gallo", "gallery",  "gal", "fyi", "futbol",
            "furniture","fund", "fujitsu", "ftr", "frontier", "frontdoor","frogans", "frl", "fresenius","fox", "foundation", "forum", "forsale", "forex", "ford","football", "foodnetwork",
            "foo", "fly", "flsmidth", "flowers", "florist", "flir", "flights", "flickr",  "fitness", "fit",  "fishing", "fish",  "firmdale","firestone", "fire","financial", "finance",
            "final", "film", "ferrero", "feedback","fedex","fast", "fashion", "farmers", "farm","fans","fan","family", "faith", "fairwinds", "fail", "fage", "extraspace", "express",
            "exposed", "expert",  "exchange", "everbank", "events", "eus",  "eurovision",  "estate",   "esq", "erni", "ericsson",  "equipment",  "epson", "epost",  "enterprises", "engineering",
            "engineer", "energy",  "emerck", "email",  "education","edu", "edeka",  "eat", "earth", "dvag",  "durban", "dupont","dunlop", "dubai",  "dtv", "drive",  "download",
            "dot","doosan", "domains",  "doha", "dog", "docs", "dnp", "discount", "directory", "direct",  "digital","diet",  "diamonds", "dhl", "dev",  "design",  "desi", "dentist",
            "dental", "democrat", "delta","deloitte","dell","delivery","degree", "deals", "dealer", "deal", "dds", "dclk", "day", "datsun", "dating", "date", "dance","dad", "dabur",
            "cyou", "cymru", "cuisinella","csc", "cruises", "crs", "crown",  "cricket", "creditunion", "creditcard","credit","courses", "coupons",  "coupon", "country","corsica", "coop",
            "cool",  "cookingchannel", "cooking", "contractors",  "contact", "consulting", "construction", "condos","comsec", "computer", "compare", "company", "community","commbank","comcast",
            "com", "cologne", "college",  "coffee", "codes","coach", "clubmed", "club", "cloud", "clothing", "clinique", "clinic","click","cleaning", "claims","cityeats", "city","citic",
            "cisco","circle", "cipriani", "church", "chrome", "christmas","chloe", "chintai", "cheap", "chat", "chase", "channel","chanel", "cfd","cfa", "cern", "ceo","center", "ceb",
            "cbre","cbn", "cba", "catering", "cat", "casino",  "cash", "casa", "cartier", "cars", "careers","career", "care", "cards", "caravan",  "car", "capital", "capetown", "canon",
            "cancerresearch","camp", "camera", "cam", "call","cal",  "cafe","cab","bzh", "buzz", "buy", "business", "builders",  "build", "bugatti", "budapest","brussels", "brother",
            "broker", "broadway", "bridgestone", "bradesco","boutique", "bot", "bostik", "bosch",  "boots","book",  "boo", "bond", "bom", "boehringer",  "boats","bnpparibas", "bnl",
            "bmw","bms", "blue","bloomberg", "blog","blanco", "blackfriday", "black", "biz", "bio", "bingo", "bing", "bike",  "bid","bible", "bharti",  "bet", "best","berlin", "bentley",
            "beer","beats", "bcn","bcg","bbva", "bbc", "bayern","bauhaus","bargains", "barefoot", "barclays", "barclaycard","barcelona", "bar","bank","band", "baidu", "baby",  "azure",
            "axa", "aws", "avianca", "autos", "auto", "author", "audio",  "audible", "audi", "auction", "attorney",  "associates",  "asia",   "arte", "art", "arpa",  "army", "archi",
            "aramco", "aquarelle", "apple", "app", "apartments", "anz","anquan", "android", "analytics","amsterdam", "amica",  "alstom",  "alsace", "ally", "allfinanz", "alipay", "alibaba",
            "akdn", "airtel", "airforce", "airbus",  "aig","agency", "agakhan", "afl",  "aetna", "aero","aeg",  "adult", "ads",   "adac", "actor", "active", "aco", "accountants",  "accountant",
            "accenture", "academy", "abudhabi", "abogado", "able","abbvie","abbott", "abb", "aarp", "aaa", "onion" };

        internal static readonly string[] Ctlds = {"한국","香港","澳門","新加坡","台灣", "台湾", "中國", "中国","გე", "ไทย", "ලංකා", "ഭാരതം", "ಭಾರತ", "భారత్","சிங்கப்பூர்",
            "இலங்கை", "இந்தியா","ଭାରତ", "ભારત","ਭਾਰਤ", "ভাৰত","ভারত", "বাংলা", "भारत", "پاکستان","مليسيا", "مصر","قطر", "فلسطين", "عمان", "عراق", "سورية","سودان",
            "تونس", "بھارت", "ایران","امارات", "المغرب", "السعودية", "الجزائر", "الاردن", "հայ",  "қаз", "укр", "срб","рф", "мон", "мкд","ею","бел", "бг",
            "ελ", "zw", "zm", "za","yt",  "ye", "ws", "wf","vu", "vn", "vi",  "vg", "ve", "vc", "va", "uz", "uy", "us", "um",  "uk", "ug","ua",  "tz", "tw","tv", "tt","tr", "tp",
            "to","tn","tm", "tl","tk","tj", "th", "tg", "tf", "td", "tc", "sz", "sy", "sx", "sv", "su","st", "ss", "sr", "so", "sn", "sm","sl",  "sk", "sj", "si","sh", "sg","se", "sd",
            "sc",  "sb","sa", "rw","ru", "rs", "ro","re", "qa", "py", "pw", "pt","ps", "pr", "pn", "pm", "pl", "pk", "ph", "pg",  "pf","pe", "pa",  "om", "nz", "nu","nr","np", "no",
            "nl", "ni", "ng","nf", "ne", "nc","na", "mz", "my", "mx", "mw", "mv", "mu", "mt", "ms", "mr", "mq", "mp", "mo", "mn", "mm",  "ml",  "mk",  "mh", "mg", "mf",  "me", "md", "mc",
            "ma","ly","lv", "lu", "lt", "ls", "lr", "lk", "li", "lc", "lb", "la", "kz",  "ky","kw", "kr", "kp", "kn", "km", "ki", "kh",  "kg", "ke", "jp",  "jo", "jm",  "je", "it", "is",
            "ir", "iq", "io", "in", "im", "il", "ie",  "id","hu", "ht", "hr",  "hn","hm", "hk", "gy", "gw", "gu", "gt", "gs", "gr","gq", "gp", "gn",  "gm", "gl", "gi",  "gh", "gg",
            "gf", "ge",  "gd", "gb", "ga", "fr", "fo", "fm",  "fk","fj", "fi", "eu", "et", "es", "er",  "eh", "eg", "ee", "ec", "dz",  "do", "dm",  "dk", "dj",  "de", "cz",  "cy",
            "cx","cw", "cv", "cu", "cr", "co", "cn",  "cm", "cl", "ck", "ci", "ch", "cg", "cf", "cd",  "cc", "ca",  "bz", "by", "bw", "bv", "bt", "bs", "br",  "bq",  "bo", "bn","bm",
            "bl", "bj","bi", "bh", "bg", "bf", "be", "bd",  "bb", "ba","az",  "ax", "aw", "au", "at", "as", "ar", "aq", "ao",  "an", "am", "al", "ai", "ag", "af",  "ae", "ad", "ac" };
    }
}
