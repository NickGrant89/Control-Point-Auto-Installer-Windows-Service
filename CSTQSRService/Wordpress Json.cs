using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCSService
{

    public class Rootobject
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public DateTime date_gmt { get; set; }
        public Guid guid { get; set; }
        public DateTime modified { get; set; }
        public DateTime modified_gmt { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public Title title { get; set; }
        public Content content { get; set; }
        public Excerpt excerpt { get; set; }
        public int author { get; set; }
        public int featured_media { get; set; }
        public string comment_status { get; set; }
        public string ping_status { get; set; }
        public bool sticky { get; set; }
        public string template { get; set; }
        public string format { get; set; }
        public object[] meta { get; set; }
        public int[] categories { get; set; }
        public object[] tags { get; set; }
        public Acf acf { get; set; }
        public _Links _links { get; set; }
    }

    public class Guid
    {
        public string rendered { get; set; }
    }

    public class Title
    {
        public string rendered { get; set; }
    }

    public class Content
    {
        public string rendered { get; set; }
        public bool _protected { get; set; }
    }

    public class Excerpt
    {
        public string rendered { get; set; }
        public bool _protected { get; set; }
    }

    public class Acf
    {
        public Monitor_Register[] monitor_register { get; set; }
        public Monitor_Configuration[] monitor_configuration { get; set; }
        public My_Device[] my_device { get; set; }
        public List<ExternalCommandCmd> external_command_cmd { get; set; }
        public string service_log_file_qsr { get; set; }
        public List<SosFile> sos_file { get; set; }
        public string monitor_post_id_qsr { get; set; }
        public string log_t { get; set; }
    }

    public class Monitor_Register
    {
        public string pc_name_onec { get; set; }
        public string ip_address_onec { get; set; }
        public string mac_address_onec { get; set; }
        public string status_onec { get; set; }
        public string post_id_onec { get; set; }
        public string time_stamp_onec { get; set; }
    }

    public class Monitor_Configuration
    {
        public string site_name_onec { get; set; }
        public string primary_ip_ocs { get; set; }
        public string backup_ip_ocs { get; set; }
        public string email_ocs { get; set; }
        public string site_endpoint { get; set; }
        public string activate_monitor { get; set; }
    }

    public class My_Device
    {
        public string windows_version { get; set; }
        public string hard_drive_space { get; set; }
        public string available_memory { get; set; }
        public string ex_ip_address { get; set; }
        public string antivirus { get; set; }
    }
    public class ExternalCommandCmd
    {
        public string external_command_qsr { get; set; }
        public bool run_command_qsr { get; set; }
    }

    public class SosFile
    {
        public bool get_sos_file_qsr { get; set; }
        public string sos_file_content { get; set; }
    }

    public class _Links
    {
        public Self[] self { get; set; }
        public Collection[] collection { get; set; }
        public About[] about { get; set; }
        public Author[] author { get; set; }
        public Reply[] replies { get; set; }
        public VersionHistory[] versionhistory { get; set; }
        public WpAttachment[] wpattachment { get; set; }
        public WpTerm[] wpterm { get; set; }
        public Cury[] curies { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Collection
    {
        public string href { get; set; }
    }

    public class About
    {
        public string href { get; set; }
    }

    public class Author
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Reply
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class VersionHistory
    {
        public string href { get; set; }
    }

    public class WpAttachment
    {
        public string href { get; set; }
    }

    public class WpTerm
    {
        public string taxonomy { get; set; }
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Cury
    {
        public string name { get; set; }
        public string href { get; set; }
        public bool templated { get; set; }
    }

}









