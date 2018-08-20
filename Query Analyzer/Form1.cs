using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Query_Analyzer
{
    public partial class Form1 : Form
    {
        public static string[] specific_words = new string[12];
        public Form1()
        {
            InitializeComponent();
            specific_words[0] = "select";
            specific_words[1] = "update";
            specific_words[2] = "create";
            specific_words[3] = "table";
            specific_words[4] = "insert";
            specific_words[5] = "into";
            specific_words[6] = "values";
            specific_words[7] = "from";
            specific_words[8] = "*";
            specific_words[9] = "delete";
            specific_words[10] = "where";
            specific_words[11] = "set";
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            input_query.BackColor = System.Drawing.Color.DarkKhaki;
            fa_rzlt.BackColor = System.Drawing.Color.DarkKhaki;
            cfg_rzlt.BackColor = System.Drawing.Color.DarkKhaki;
            sem_rzlt.BackColor = System.Drawing.Color.DarkKhaki;
            tab_rzlt.BackColor = System.Drawing.Color.DarkKhaki;
            component_rzlt.BackColor = System.Drawing.Color.DarkKhaki;
            fa_rzlt.Text = "";
            tab_rzlt.Text = "";
            component_rzlt.Text = "";
            string tokens_of_query = "";
            List<string> query_word = new List<string> { };
            List<string> query_grammer = new List<string> { };
            int cfg_result = 0;
            string cfg = "";
            string query = input_query.Text;
            char[] converted_query = query.ToCharArray();
            int t_spaces = 0;
            int t_commas = 0;
            for (int i = 0; i < query.Length; i++)
            {
                if (converted_query[i] == ' ')
                {
                    t_spaces++;
                }

            }

                                    /**********Checks for Grammer************/

            /////////////////////////////////////////////////////////////////////////////////////////

            string[] split_on_space = query.Split(' ');
            for (int i = 0; i <= t_spaces; i++)
            {
                query_word.Add(split_on_space[i]);
            }
            foreach (string s in query_word)
            {
                int comma_counter = 0;
                char[] word = s.ToCharArray();
                for (int i = 0; i < s.Length; i++)
                {
                    if (word[i] == ',')
                    {
                        t_commas++;
                    }
                }
                string[] split_on_comma = s.Split(',');
                for (int i = 0; i <= t_commas; i++)
                {
                    query_grammer.Add(split_on_comma[i]);
                    if (comma_counter < t_commas)
                    {
                        query_grammer.Add(",");
                        comma_counter++;
                    }
                }
                t_commas = 0;
            }
            query_word.Clear();
            foreach (string s in query_grammer)
            {
                string r_parenthesis = "";
                char[] word = s.ToCharArray();
                for (int i = 0; i < s.Length; i++)
                {
                    if (word[i] != '(')
                    {
                        r_parenthesis += word[i];
                    }
                    else
                    {
                        query_word.Add("(");
                    }
                }
                query_word.Add(r_parenthesis);
            }
            int flags = 1;
            query_grammer.Clear();
            foreach (string s in query_word)
            {
                string l_parenthesis = "";
                char[] word = s.ToCharArray();
                for (int i = 0; i < s.Length; i++)
                {
                    if (word[i] != ')')
                    {
                        l_parenthesis += word[i];
                    }
                    else
                    {
                        flags = 2;
                    }
                }
                query_grammer.Add(l_parenthesis);
                if (flags == 2)
                {
                    query_grammer.Add(")");
                    flags = 1;
                }
            }
            query_word.Clear();
            foreach (string s in query_grammer)
            {
                string back_slash = "";
                char[] word = s.ToCharArray();
                for (int i = 0; i < s.Length; i++)
                {
                    if (word[i] != '\'')
                    {
                        back_slash += word[i];
                    }
                }
                try
                {
                    if (word[0] == '\'')
                    {
                        query_word.Add("\'");
                    }
                    query_word.Add(back_slash);
                    if (word[s.Length - 1] == '\'')
                    {
                        query_word.Add("\'");
                        flags = 1;
                    }
                }
                catch
                {
                    if (query == "")
                    {
                        MessageBox.Show("Required field is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Extra spaces are added", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    } 
                    break;
                }
            }
            query_grammer.Clear();

            ///////////////////////////////////////////////////////////////////////////////////////////////

                                 /**********Some coding for FA's************/

            ///////////////////////////////////////////////////////////////////////////////////////////////

            int flag_for_check_error = 1;
            try
            {
                foreach (string s in query_word)
                {
                    if ((s != "<=") && (s != ">="))
                    {
                        char[] tokens = s.ToCharArray();
                        int flag = 1;
                        int state = 0;
                        if (tokens[0].ToString().Equals("s"))
                        {
                            if ((tokens[0].ToString().Equals("s")) && (tokens[2].ToString().Equals("t")))
                            {
                                state = 11;
                                fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                            }
                            else
                            {
                                state = 0;
                                fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                            }
                        }
                        else if (tokens[0].ToString().Equals("u"))
                        {
                            state = 1;
                            fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                        }
                        else if (tokens[0].ToString().Equals("c"))
                        {
                            state = 2;
                            fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                        }
                        else if (tokens[0].ToString().Equals("t"))
                        {
                            state = 3;
                            fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                        }
                        else if (tokens[0].ToString().Equals("i"))
                        {
                            if (s.Length == 4)
                            {
                                if ((tokens[0].ToString().Equals("i")) && (tokens[3].ToString().Equals("o")))
                                {
                                    state = 5;
                                    fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                                }
                            }
                            else if (s.Length == 3)
                            {
                                if ((tokens[0].ToString().Equals("i")) && (tokens[1].ToString().Equals("n")))
                                {
                                    tokens_of_query += "Data-Type ";
                                }
                            }
                            else
                            {
                                state = 4;
                                fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                            }
                        }
                        else if (tokens[0].ToString().Equals("d"))
                        {
                            state = 9;
                            fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                        }
                        else if (tokens[0].ToString().Equals("f"))
                        {
                            state = 7;
                            fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                        }
                        else if (tokens[0].ToString().Equals("v"))
                        {
                            if (s.Length == 7)
                            {
                                if ((tokens[0].ToString().Equals("v") && (tokens[2].ToString().Equals("r"))))
                                {
                                    tokens_of_query += "Data-Type ";
                                }
                            }
                            else
                            {
                                state = 6;
                                fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                            }
                        }
                        else if (tokens[0].ToString().Equals("*"))
                        {
                            state = 8;
                            fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                        }
                        else if (tokens[0].ToString().Equals("w"))
                        {
                            state = 10;
                            fa_maker_of_query(ref tokens_of_query, s, tokens, ref flag, state);
                        }
                        else if (tokens[0] == ',')
                        {
                            tokens_of_query += "Comma ";
                        }
                        else if (tokens[0] == ';')
                        {
                            tokens_of_query += "Semicolon ";
                        }

                        else if (tokens[0] == '=')
                        {
                            tokens_of_query += "Equal ";
                        }
                        else if (tokens[0] == '>')
                        {
                            tokens_of_query += "Greater ";
                        }
                        else if (tokens[0] == '<')
                        {
                            tokens_of_query += "Smaller ";
                        }
                        else if ((tokens[0] == ')') || (tokens[0] == '('))
                        {
                            tokens_of_query += "Bracket ";
                        }
                        else if (tokens[0] == '\'')
                        {
                            tokens_of_query += "Back-Slash ";
                        }
                        else if (tokens[0] != ',')
                        {
                            tokens_of_query += "Identifier ";
                        }
                    }
                    else if (s == "<=")
                    {
                        tokens_of_query += "Smaller Equal ";
                    }
                    else if (s == ">=")
                    {
                        tokens_of_query += "Greater Equal ";
                    }
                }
            }
            catch
            {
                flag_for_check_error = 2;
                MessageBox.Show("Extra spaces are added.", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                query_word.Clear();
            }
            if (flag_for_check_error == 1)
            {
                int check_for_spaces = 0;
                fa_rzlt.Text = tokens_of_query;
                char[] sp_occurence = tokens_of_query.ToCharArray();
                for (int i = 0; i < tokens_of_query.Length; i++)
                {
                    if (sp_occurence[i] == ' ')
                    {
                        check_for_spaces++;
                    }
                }
                string[] array_for_cfg_checking = tokens_of_query.Split(' ');

                foreach (string s in query_word)
                {
                    component_rzlt.Text += s + "\r\n";
                }

                //////////////////////////////////////////////////////////////////////

                /************Context Free Grammer (CFG) of all queries***************/

                //////////////////////////////////////////////////////////////////////


                                 /**********CFG for Select************/

                try
                {
                    if (query_word[0].ToString().Equals(specific_words[0]))
                    {
                        if ((check_for_spaces == 5) && (array_for_cfg_checking[1] == "Key-Word"))
                        {
                            if (array_for_cfg_checking[0] == "Key-Word")
                            {
                                if (array_for_cfg_checking[1] == "Key-Word")
                                {
                                    if (array_for_cfg_checking[2] == "Key-Word")
                                    {
                                        if (array_for_cfg_checking[3] == "Identifier")
                                        {
                                            cfg += '3';
                                            if (array_for_cfg_checking[4] == "Semicolon")
                                            {
                                                cfg_result = 1;
                                                cfg_rzlt.Text = "Correct Syntex!!!";
                                            }
                                            else
                                            {
                                                cfg_rzlt.Text = "Invalid query!!!  May be semicolon is missing :(";
                                            }
                                        }
                                        else
                                        {
                                            cfg_rzlt.Text = "Invalid query!!! ";
                                        }
                                    }
                                    else
                                    {
                                        cfg_rzlt.Text = "Invalid query!!! ";
                                    }
                                }
                                else
                                {
                                    cfg_rzlt.Text = "Invalid query!!! ";
                                }
                            }
                            else
                            {
                                cfg_rzlt.Text = "Invalid query!!! ";
                            }
                        }
                        else if (check_for_spaces >= 5)
                        {
                            int comma_count = 0, ident_count = 0, loop_count = 0;
                            if (array_for_cfg_checking[0] == "Key-Word")
                            {
                                while (array_for_cfg_checking[(1 + loop_count)] != "Key-Word")
                                {
                                    if (array_for_cfg_checking[(1 + loop_count)] == "Identifier")
                                    {
                                        cfg += (1 + loop_count) + " ";
                                        ident_count++;
                                    }
                                    else if ((array_for_cfg_checking[(1 + loop_count)] == "Comma") && (comma_count < ident_count))
                                    {
                                        comma_count++;
                                    }
                                    loop_count++;
                                }
                                int loop_value = 1 + loop_count;
                                if (((comma_count + 1) == ident_count) && (array_for_cfg_checking[(1 + loop_count)] == "Key-Word"))
                                {
                                    if (array_for_cfg_checking[1] != "Comma")
                                    {
                                        if (array_for_cfg_checking[(1 + loop_value)] == "Identifier")
                                        {
                                            cfg += (1 + loop_value) + " ";

                                            if (array_for_cfg_checking[(2 + loop_value)] == "Semicolon")
                                            {
                                                cfg_result = 2;
                                                cfg_rzlt.Text = "Correct Syntex!!!";
                                            }
                                            else if (array_for_cfg_checking[(2 + loop_value)] == "Key-Word")
                                            {
                                                if (array_for_cfg_checking[(3 + loop_value)] == "Identifier")
                                                {

                                                    int geater_count = 0, smaller_count = 0, eqal_count = 0, loop_counter = 0;
                                                    while (((!array_for_cfg_checking[(4 + loop_value + loop_counter)].Equals("Identifier"))))
                                                    {
                                                        if ((array_for_cfg_checking[(4 + loop_value + loop_counter)] == "Greater"))
                                                        {
                                                            geater_count++;
                                                        }
                                                        else if ((array_for_cfg_checking[(4 + loop_value + loop_counter)] == "Smaller"))
                                                        {
                                                            smaller_count++;
                                                        }
                                                        else if ((array_for_cfg_checking[(4 + loop_value + loop_counter)] == "Equal"))
                                                        {
                                                            eqal_count++;
                                                        }
                                                        loop_counter++;
                                                    }
                                                    int loop_nested_counter = 4 + loop_value + loop_counter;
                                                    if (((geater_count == 1) && (smaller_count == 0) && ((eqal_count == 1) || (eqal_count == 0))) || ((geater_count == 0) && (smaller_count == 1) && ((eqal_count == 1) || (eqal_count == 0))) || (((eqal_count == 1))))
                                                    {

                                                        if (array_for_cfg_checking[(loop_nested_counter)] == "Identifier")
                                                        {
                                                            if (array_for_cfg_checking[(loop_nested_counter - 1)] == "Back-Slash")
                                                            {
                                                                if (array_for_cfg_checking[(loop_nested_counter)] == "Identifier")
                                                                {
                                                                    if (array_for_cfg_checking[(loop_nested_counter + 1)] == "Back-Slash")
                                                                    {
                                                                        if (array_for_cfg_checking[(loop_nested_counter + 2)] == "Semicolon")
                                                                        {
                                                                            cfg_result = 3;
                                                                            cfg_rzlt.Text = "Correct Syntex!!!";
                                                                        }
                                                                        else
                                                                        {
                                                                            cfg_rzlt.Text = "Invalid query!!! May be a semicolon is missing :(";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        cfg_rzlt.Text = "Invalid query!!!";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    cfg_rzlt.Text = "Invalid query!!!";
                                                                }
                                                            }
                                                            else if (array_for_cfg_checking[(1 + loop_nested_counter)] == "Semicolon")
                                                            {
                                                                cfg_result = 4;
                                                                cfg_rzlt.Text = "Correct Syntex!!!";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        cfg_rzlt.Text = "Invalid query!!!";
                                                    }
                                                }
                                                else
                                                {
                                                    cfg_rzlt.Text = "Invalid query!!!";
                                                }
                                            }
                                            else
                                            {
                                                cfg_rzlt.Text = "Invalid query!!!";
                                            }
                                        }
                                        else
                                        {
                                            cfg_rzlt.Text = "Invalid query!!!";
                                        }
                                    }
                                    else
                                    {
                                        cfg_rzlt.Text = "Invalid query!!!";
                                    }
                                }
                                else
                                {
                                    cfg_rzlt.Text = "Invalid query!!!";
                                }
                            }
                            else
                            {
                                cfg_rzlt.Text = "Invalid query!!!";
                            }
                        }
                        else if (check_for_spaces < 5)
                        {
                            cfg_rzlt.Text = "Invalid query!!!";
                        }
                    }

                          /**********CFG for Update************/

                    else if (query_word[0].ToString().Equals(specific_words[1]))
                    {
                        if (array_for_cfg_checking[0] == "Key-Word")
                        {
                            if (array_for_cfg_checking[1] == "Identifier")
                            {
                                if (array_for_cfg_checking[2] == "Key-Word")
                                {
                                    if (array_for_cfg_checking[3] == "Identifier")
                                    {
                                        if (array_for_cfg_checking[4] == "Equal")
                                        {
                                            if (array_for_cfg_checking[5] == "Identifier")
                                            {
                                                if (array_for_cfg_checking[6] == "Key-Word")
                                                {
                                                    if (array_for_cfg_checking[7] == "Identifier")
                                                    {
                                                        if (array_for_cfg_checking[8] == "Equal")
                                                        {
                                                            if (array_for_cfg_checking[9] == "Identifier")
                                                            {
                                                                if (array_for_cfg_checking[10] == "Semicolon")
                                                                {
                                                                    cfg_result = 5;
                                                                    cfg_rzlt.Text = "Correct Syntex!!!";
                                                                }
                                                                else
                                                                {
                                                                    cfg_rzlt.Text = "Invalid query!!! May be a semicolon is missing :(";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                cfg_rzlt.Text = "Invalid query!!!";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            cfg_rzlt.Text = "Invalid query!!!";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        cfg_rzlt.Text = "Invalid query!!!";
                                                    }
                                                }
                                                else
                                                {
                                                    cfg_rzlt.Text = "Invalid query!!!";
                                                }
                                            }
                                            else
                                            {
                                                cfg_rzlt.Text = "Invalid query!!!";
                                            }
                                        }
                                        else
                                        {
                                            cfg_rzlt.Text = "Invalid query!!!";
                                        }
                                    }
                                    else
                                    {
                                        cfg_rzlt.Text = "Invalid query!!!";
                                    }
                                }
                                else
                                {
                                    cfg_rzlt.Text = "Invalid query!!!";
                                }
                            }
                            else
                            {
                                cfg_rzlt.Text = "Invalid query!!!";
                            }
                        }
                        else
                        {
                            cfg_rzlt.Text = "Invalid query!!!";
                        }
                    }

                          /**********CFG for Create************/

                    else if (query_word[0].ToString().Equals(specific_words[2]))
                    {
                        if (check_for_spaces >= 7)
                        {
                            cfg = "";
                            if (array_for_cfg_checking[0] == "Key-Word")
                            {
                                if (array_for_cfg_checking[1] == "Key-Word")
                                {
                                    if (array_for_cfg_checking[2] == "Identifier")
                                    {
                                        cfg = cfg + "2" + " ";
                                        if (array_for_cfg_checking[3] == "Bracket")
                                        {
                                            int ident_count_token = 0, dtype_count_token = 0, comma_count_token = 0, increse_value = 0;
                                            try
                                            {
                                                while ((array_for_cfg_checking[4 + increse_value] != "Bracket") && ((4 + increse_value) < check_for_spaces))
                                                {
                                                    if (array_for_cfg_checking[4 + increse_value] == "Identifier")
                                                    {
                                                        cfg = cfg + (4 + increse_value) + " ";
                                                        ident_count_token++;
                                                    }
                                                    else if ((array_for_cfg_checking[4 + increse_value] == "Data-Type") && (dtype_count_token < ident_count_token))
                                                    {
                                                        dtype_count_token++;
                                                    }
                                                    else if (array_for_cfg_checking[4 + increse_value] == "Comma")
                                                    {
                                                        comma_count_token++;
                                                    }
                                                    increse_value++;
                                                }
                                                if ((ident_count_token == dtype_count_token) && (array_for_cfg_checking[4 + increse_value] == "Bracket") && (comma_count_token == (dtype_count_token - 1)) && (array_for_cfg_checking[5 + increse_value] == "Semicolon"))
                                                {
                                                    cfg_result = 6;
                                                    cfg_rzlt.Text = "Correct Syntex!!!";
                                                }
                                                else
                                                {
                                                    cfg_rzlt.Text = "Invalid query!!!";
                                                }
                                            }
                                            catch
                                            {
                                                cfg_rzlt.Text = "Invalid query!!!";
                                            }
                                        }
                                        else
                                        {
                                            cfg_rzlt.Text = "Invalid query!!!";
                                        }
                                    }
                                    else
                                    {
                                        cfg_rzlt.Text = "Invalid query!!!";
                                    }
                                }
                                else
                                {
                                    cfg_rzlt.Text = "Invalid query!!!";
                                }
                            }
                            else
                            {
                                cfg_rzlt.Text = "Invalid query!!!";
                            }
                        }
                        else
                        {
                            cfg_rzlt.Text = "Invalid query!!!";
                        }
                    }

                         /**********CFG for Insert************/

                    else if (query_word[0].ToString().Equals(specific_words[4]))
                    {
                        if (check_for_spaces >= 7)
                        {
                            if (array_for_cfg_checking[0] == "Key-Word")
                            {
                                if (array_for_cfg_checking[1] == "Key-Word")
                                {
                                    if (array_for_cfg_checking[2] == "Identifier")
                                    {
                                        cfg += "2 ";
                                        if (array_for_cfg_checking[3] == "Key-Word")
                                        {
                                            if (array_for_cfg_checking[4] == "Bracket")
                                            {
                                                int ident_count_token = 0, qout_count_token = 0, comma_count_token = 0, increse_value = 0;
                                                try
                                                {
                                                    while ((array_for_cfg_checking[5 + increse_value] != "Bracket") && ((5 + increse_value) < check_for_spaces))
                                                    {
                                                        if (array_for_cfg_checking[5 + increse_value] == "Identifier")
                                                        {
                                                            cfg += (5 + increse_value) + " ";
                                                            ident_count_token++;
                                                        }
                                                        else if ((array_for_cfg_checking[5 + increse_value] == "Back-Slash"))
                                                        {
                                                            qout_count_token++;
                                                        }
                                                        else if (array_for_cfg_checking[5 + increse_value] == "Comma")
                                                        {
                                                            comma_count_token++;
                                                        }
                                                        increse_value++;
                                                    }
                                                    if (((qout_count_token % 2) == 0) && (comma_count_token == (ident_count_token - 1)))
                                                    {
                                                        if ((array_for_cfg_checking[5 + increse_value] == "Bracket"))
                                                        {
                                                            if ((array_for_cfg_checking[6 + increse_value] == "Semicolon"))
                                                            {
                                                                cfg_result = 7;
                                                                cfg_rzlt.Text = "Correct Syntex!!!";
                                                            }
                                                            else
                                                            {
                                                                cfg_rzlt.Text = "Invalid query!!! May be a semicolon is missing :(";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            cfg_rzlt.Text = "Invalid query!!!";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        cfg_rzlt.Text = "Invalid query!!!";
                                                    }
                                                }
                                                catch
                                                {
                                                    cfg_rzlt.Text = "Invalid query!!!";
                                                }
                                            }
                                            else
                                            {
                                                cfg_rzlt.Text = "Invalid query!!!";
                                            }
                                        }
                                        else
                                        {
                                            cfg_rzlt.Text = "Invalid query!!!";
                                        }
                                    }
                                    else
                                    {
                                        cfg_rzlt.Text = "Invalid query!!!";
                                    }
                                }
                                else
                                {
                                    cfg_rzlt.Text = "Invalid query!!!";
                                }
                            }
                            else
                            {
                                cfg_rzlt.Text = "Invalid query!!!";
                            }
                        }
                        else
                        {
                            cfg_rzlt.Text = "Invalid query!!!";
                        }
                    }

                         /**********CFG for Delete************/

                    else if (query_word[0].ToString().Equals(specific_words[9]))
                    {
                        cfg = "";
                        if (check_for_spaces == 3)
                        {
                            if (array_for_cfg_checking[0] == "Key-Word")
                            {
                                if (array_for_cfg_checking[1] == "Identifier")
                                {
                                    cfg = "1";
                                    if (array_for_cfg_checking[2] == "Semicolon")
                                    {
                                        cfg_result = 8;
                                        cfg_rzlt.Text = "Correct Syntex!!!";
                                    }
                                    else
                                    {
                                        cfg_rzlt.Text = "Invalid query!!! May be a semicolon is missing :(";
                                    }
                                }
                                else
                                {
                                    cfg_rzlt.Text = "Invalid query!!!";
                                }
                            }
                            else
                            {
                                cfg_rzlt.Text = "Invalid query!!!";
                            }
                        }
                        else if (check_for_spaces > 3)
                        {
                            if (array_for_cfg_checking[0] == "Key-Word")
                            {
                                if (array_for_cfg_checking[1] == "Key-Word")
                                {
                                    if (array_for_cfg_checking[2] == "Identifier")
                                    {
                                        cfg += "2 ";
                                        if (array_for_cfg_checking[3] == "Key-Word")
                                        {
                                            if (array_for_cfg_checking[4] == "Identifier")
                                            {
                                                cfg += "4 ";
                                                int geater_count = 0, smaller_count = 0, eqal_count = 0, loop_counter = 0;
                                                while (((!array_for_cfg_checking[(5 + loop_counter)].Equals("Identifier"))))
                                                {
                                                    if ((array_for_cfg_checking[(5 + loop_counter)] == "Greater"))
                                                    {
                                                        geater_count++;
                                                    }
                                                    else if ((array_for_cfg_checking[(5 + loop_counter)] == "Smaller"))
                                                    {
                                                        smaller_count++;
                                                    }
                                                    else if ((array_for_cfg_checking[(5 + loop_counter)] == "Equal"))
                                                    {
                                                        eqal_count++;
                                                    }
                                                    loop_counter++;
                                                }
                                                int loop_nested_counter = 5 + loop_counter;
                                                if (((geater_count == 1) && (smaller_count == 0) && ((eqal_count == 1) || (eqal_count == 0))) || ((geater_count == 0) && (smaller_count == 1) && ((eqal_count == 1) || (eqal_count == 0))) || (((eqal_count == 1))))
                                                {
                                                    if (array_for_cfg_checking[(loop_nested_counter)] == "Identifier")
                                                    {
                                                        cfg += (loop_nested_counter).ToString() + " ";
                                                        if (array_for_cfg_checking[(loop_nested_counter - 1)] == "Back-Slash")
                                                        {
                                                            if (array_for_cfg_checking[(loop_nested_counter)] == "Identifier")
                                                            {
                                                                if (array_for_cfg_checking[(loop_nested_counter + 1)] == "Back-Slash")
                                                                {
                                                                    if (array_for_cfg_checking[(loop_nested_counter + 2)] == "Semicolon")
                                                                    {
                                                                        cfg_result = 9;
                                                                        cfg_rzlt.Text = "Correct Syntex!!!";
                                                                    }
                                                                    else
                                                                    {
                                                                        cfg_rzlt.Text = "Invalid query!!! May be a semicolon is missing :(";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    cfg_rzlt.Text = "Invalid query!!!";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                cfg_rzlt.Text = "Invalid query!!!";
                                                            }
                                                        }
                                                        else if (array_for_cfg_checking[(1 + loop_nested_counter)] == "Semicolon")
                                                        {
                                                            cfg_result = 10;
                                                            cfg_rzlt.Text = "Correct Syntex!!!";
                                                        }
                                                        else
                                                        {
                                                            cfg_rzlt.Text = "Invalid query!!!";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    cfg_rzlt.Text = "Invalid query!!!";
                                                }
                                            }
                                            else
                                            {
                                                cfg_rzlt.Text = "Invalid query!!!";
                                            }
                                        }
                                        else
                                        {
                                            cfg_rzlt.Text = "Invalid query!!!";
                                        }
                                    }
                                    else
                                    {
                                        cfg_rzlt.Text = "Invalid query!!!";
                                    }
                                }
                                else
                                {
                                    cfg_rzlt.Text = "Invalid query!!!";
                                }
                            }
                            else
                            {
                                cfg_rzlt.Text = "Invalid query!!!";
                            }
                        }
                        else if (check_for_spaces <= 3)
                        {
                            cfg_rzlt.Text = "Invalid query!!!";
                        }
                    }
                    else
                    {
                        cfg_rzlt.Text = "Invalid query!!!";
                    }
                }
                catch
                {
                    cfg_rzlt.Text = "Invalid query!!!";
                }
            }

            //////////////////////////////////////////////////////////////////////

            /***************Semantic Analysis & File Handling********************/

            //////////////////////////////////////////////////////////////////////

            string select_from_table = "";
            string select_from_data = "";
            int sign_for_work_done = 1;
            string path = Directory.GetCurrentDirectory();
            path += "\\"+"Schema.txt";

            /*****************Code for create statement*******************/

            if (cfg_result == 6)
            {
                int count1 = 0, count2 = 0;
                string[] array_for_semantic_check = cfg.Split(' ');
                foreach (string s in query_word)
                {
                    if (count1.ToString() == array_for_semantic_check[count2])
                    {
                        select_from_table = select_from_table + s + " ";
                        count2++;
                        select_from_data = select_from_data + s + " ";
                    }
                    count1++;
                }
                Boolean flag = false;
                try
                {
                    using (var obj_for_reader = new StreamReader(path, true))
                    {
                        string row;
                        string[] single_row;
                        string[] multiple_row;
                        string[] sub_extraxt = select_from_table.Split(' ');
                        while ((row = obj_for_reader.ReadLine()) != null)
                        {
                            single_row = row.Split(':');
                            if (single_row[0] == "T")
                            {
                                multiple_row = single_row[1].Split('#');
                                if (multiple_row[0] == sub_extraxt[0])
                                {
                                    sign_for_work_done = 1;
                                    break;
                                }
                                else
                                {
                                    sign_for_work_done = 2;
                                }
                                flag = true;
                            }
                        }
                        if ((row == null) && (flag == false))
                        {
                            sign_for_work_done = 2;
                        }
                    }
                }
                catch
                {
                    sem_rzlt.Text = "File does not exist!!!";
                }
                using (var obj_for_writer = new StreamWriter(path, true))
                {
                    string get_data = "";
                    char[] get_row = select_from_table.ToCharArray();
                    for (int i = 0; i < select_from_table.Length; i++)
                    {
                        if (get_row[i] == ' ')
                        {
                            get_row[i] = '#';
                        }
                    }
                    for (int i = 0; i < select_from_table.Length; i++)
                    {
                        get_data += get_row[i];
                    }
                    int is_data_exists = 1;
                    int is_table_exists = 0;
                    char[] selected_data = select_from_data.ToCharArray();
                    for (int i = 0; i < select_from_data.Length; i++)
                    {
                        if (selected_data[i] == ' ')
                        {
                            is_table_exists++;
                        }
                    }
                    string[] array_for_readed_data = select_from_data.Split(' ');
                    for (int i = 0; i < is_table_exists; i++)
                    {
                        for (int j = 0; j < is_table_exists; j++)
                        {
                            if ((array_for_readed_data[i].Equals(array_for_readed_data[j])) && (j != i))
                            {
                                is_data_exists = 2;
                            }
                        }
                    }
                    if (!(is_data_exists == 2))
                    {
                        if (sign_for_work_done == 2)
                        {
                            sem_rzlt.Text = "Table created successfully!!!";
                            obj_for_writer.WriteLine("T:" + get_data);
                        }
                        else
                        {
                            sem_rzlt.Text = "Table already exists!!!";
                        }
                    }
                    else
                    {
                        sem_rzlt.Text = "Not successfull becaues of 'Column' or 'Table' redundancy!!!";
                    }
                }
            }

                /****************Code for Delete statament***********************/

            else if (cfg_result == 8)
            {
                int index = 0, check = 1;
                foreach (string s in query_word)
                {
                    if (index.ToString() == cfg)
                    {
                        select_from_table = s;
                    }
                    index++;
                }
                Boolean flag = false;
                string temporary_file = Path.GetTempFileName();
                try
                {
                    using (var obj_for_reader = new StreamReader(path))
                    {
                        using (var obj_for_writer = new StreamWriter(temporary_file, true))
                        {
                            string row;
                            string[] single_row;
                            string[] multiple_row;
                            string[] sub_extraxt = select_from_table.Split(' ');
                            while ((row = obj_for_reader.ReadLine()) != null)
                            {
                                single_row = row.Split(':');
                                if ((single_row[0] == "T") || (single_row[0] == "D"))
                                {
                                    multiple_row = single_row[1].Split('#');
                                    if (multiple_row[0] != sub_extraxt[0])
                                    {
                                        flag = true;
                                        check = 2;
                                        obj_for_writer.WriteLine(row);
                                    }
                                }
                            }
                            if ((row == null) && (flag == false))
                            {
                                sem_rzlt.Text = "Table not found!!!";
                            }
                            if (check == 2)
                            {
                                sem_rzlt.Text = "Deleted successfully";
                            }
                        }
                    }
                    File.Delete(path);
                    File.Move(temporary_file, path);
                }
                catch
                {
                    sem_rzlt.Text = "Table not found!!!";
                }
            }
            else if ((cfg_result == 10) || (cfg_result == 9))
            {
                int t_data = 0;
                int t_hashes = 0;
                string[] space_splitted_array = cfg.Split(' ');
                int tab_position = Convert.ToInt32(space_splitted_array[0]);
                int col_position = Convert.ToInt32(space_splitted_array[1]);
                int position = Convert.ToInt32(space_splitted_array[2]);
                try
                {
                    using (var obj_for_reader = new StreamReader(path))
                    {
                        string row;
                        string[] single_row;
                        string[] multiple_row;
                        char[] total_hashes;
                        while ((row = obj_for_reader.ReadLine()) != null)
                        {
                            single_row = row.Split(':');
                            if ((single_row[0] == "T"))
                            {
                                total_hashes = single_row[1].ToCharArray();
                                for (int i = 0; i < single_row[1].Length; i++)
                                {
                                    if (total_hashes[i] == '#')
                                    {
                                        t_hashes++;
                                    }
                                }
                                multiple_row = single_row[1].Split('#');
                                if (multiple_row[0] == query_word[tab_position])
                                {
                                    try
                                    {
                                        for (int j = 1; j < t_hashes; j++)
                                        {
                                            if (multiple_row[0] == query_word[col_position])
                                            {
                                                sem_rzlt.Text = "There should be column name in condition";
                                                break;
                                            }
                                            else if (multiple_row[j].Equals(query_word[col_position], StringComparison.OrdinalIgnoreCase) == true)
                                            {
                                                t_data = j;
                                                break;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        sem_rzlt.Text = "Column does not exist!!!";
                                    }
                                }
                            }
                        }
                    }
                    string temporary_file = Path.GetTempFileName();
                    if (t_data != 0)
                    {
                        using (var obj_for_reader = new StreamReader(path))
                        {
                            using (var obj_for_writer = new StreamWriter(temporary_file, true))
                            {
                                string line1;
                                string[] sub_line1;
                                string[] sub_nested_line1;
                                int check = 2;
                                while ((line1 = obj_for_reader.ReadLine()) != null)
                                {
                                    sub_line1 = line1.Split(':');
                                    if ((sub_line1[0] == "D") || (sub_line1[0] == "T"))
                                    {
                                        sub_nested_line1 = sub_line1[1].Split('#');
                                        if (sub_nested_line1[t_data] != query_word[position])
                                        {
                                            check = 1;
                                            obj_for_writer.WriteLine(line1);
                                        }
                                    }
                                }
                                if (check == 1)
                                {
                                    sem_rzlt.Text = "Deleted successfully!!!";
                                }
                            }
                        }
                    }
                    File.Delete(path);
                    File.Move(temporary_file, path);
                }
                catch
                {
                    sem_rzlt.Text = "File does not exists!!!";
                }
            }

                /***************Code for Select statements*******************/

            else if (cfg_result == 1)   
            {
                tab_rzlt.Text = "";
                try
                {
                    using (var obj_for_reader = new StreamReader(path))
                    {
                        string row;
                        string[] single_row;
                        string[] multiple_row;
                        char[] single_row_no;
                        int symbol_checker = 0;
                        int table_value = 1;
                        while ((row = obj_for_reader.ReadLine()) != null)
                        {
                            single_row = row.Split(':');
                            if ((single_row[0] == "D") || (single_row[0] == "T"))
                            {
                                multiple_row = single_row[1].Split('#');
                                if (multiple_row[0] == query_word[3])
                                {
                                    try
                                    {
                                        single_row_no = single_row[1].ToCharArray();
                                        for (int i = 0; i < single_row[1].Length; i++)
                                        {
                                            if (single_row_no[i] == '#')
                                            {
                                                symbol_checker++;
                                            }
                                        }
                                        //string[] arr1 = row.Split('D');
                                        //string[] arr2 = arr1[0].Split('#');
                                        //dataGridView1.ColumnCount = arr2.Length - 1;
                                        //for (int i = 1; i < arr2.Length; i++)
                                        //{
                                        //    dataGridView1.Columns[i].Name = arr2[i];
                                        //    //  MessageBox.Show(arr2[i]);
                                        //}
                                        string data = "";
                                        for (int i = 1; i < symbol_checker; i++)
                                        {
                                            data += multiple_row[i] + " "; 
                                            table_value = 2;
                                        }
                                        tab_rzlt.Text += data + "\r\n";
                                        sem_rzlt.Text = "Data retrieved successfully!!!";
                                       
                                        data = "";
                                        symbol_checker = 0;
                                    }
                                    catch
                                    {
                                        tab_rzlt.Text = "Error in data retrieval!!!";
                                    }
                                }
                            }
                        }
                        if (table_value == 1)
                        {
                            tab_rzlt.Text = "Table not found!!!";
                            sem_rzlt.Text = "No data found!!!";
                        }
                    }
                }
                catch
                {
                    tab_rzlt.Text = "File does not exists!!!";
                    sem_rzlt.Text = "File does not exists!!!";
                }
            }

                /*****************Code for Insert statement**********************/

            else if (cfg_result == 7)
            {
                int symbol_checker = 0;
                int no_space_count = 0;
                string temporary_file = Path.GetTempFileName();
                try
                {
                    using (var obj_for_reader = new StreamReader(path))
                    {
                        string row;
                        string[] single_row;
                        string[] multiple_row;
                        char[] single_row_no;
                        char[] rows_counter;
                        rows_counter = cfg.ToCharArray();
                        for (int i = 0; i < cfg.Length; i++)
                        {
                            if (rows_counter[i] == ' ')
                            {
                                no_space_count++;
                            }
                        }
                        int count1 = 0, count2 = 0;
                        string[] array_for_semantic_check = cfg.Split(' ');
                        select_from_table += "D:";
                        foreach (string s in query_word)
                        {
                            if (count1.ToString() == array_for_semantic_check[count2])
                            {
                                select_from_table = select_from_table + s + "#";
                                count2++;
                            }
                            count1++;
                        }
                        while ((row = obj_for_reader.ReadLine()) != null)
                        {
                            single_row = row.Split(':');
                            if ((single_row[0] == "T"))
                            {
                                multiple_row = single_row[1].Split('#');
                                if (multiple_row[0] == query_word[2])
                                {
                                    single_row_no = single_row[1].ToCharArray();
                                    for (int i = 0; i < single_row[1].Length; i++)
                                    {
                                        if (single_row_no[i] == '#')
                                        {
                                            symbol_checker++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (symbol_checker == no_space_count)
                    {
                        int tab_check = 1;
                        using (var obj_for_reader = new StreamReader(path))
                        {
                            using (var obj_for_writer = new StreamWriter(temporary_file, true))
                            {
                                string row;
                                string[] single_row;
                                string[] multiple_row;
                                while ((row = obj_for_reader.ReadLine()) != null)
                                {
                                    single_row = row.Split(':');
                                    if ((single_row[0] == "T"))
                                    {
                                        multiple_row = single_row[1].Split('#');
                                        if (multiple_row[0] == query_word[2])
                                        {
                                            tab_check = 2;
                                        }
                                        if ((multiple_row[0] != query_word[2]) && (tab_check == 2))
                                        {
                                            obj_for_writer.WriteLine(select_from_table);
                                            tab_check = 1;
                                        }
                                    }
                                    obj_for_writer.WriteLine(row);
                                }
                                if (tab_check == 2)
                                {
                                    obj_for_writer.WriteLine(select_from_table);
                                    sem_rzlt.Text = "Data inserted successfully!!!";
                                }
                            }
                        }
                        File.Delete(path);
                        File.Move(temporary_file, path);
                    }
                    else
                    {
                        sem_rzlt.Text = "May be table or columns are not same!!!";
                    }
                }
                catch
                {
                    tab_rzlt.Text = "File does not exist!!!";
                }
            }
                
                /*******************Code for Update statement************************/

            else if (cfg_result == 5)
            {
                string temporary_file = Path.GetTempFileName();
                string final_result = "";
                int check = 1, check1 = 1;
                try
                {
                    using (var obj_for_reader = new StreamReader(path))
                    {
                        string row;
                        string[] single_row;
                        string[] multiple_row;
                        char[] single_row_no;
                        int symbol_checker = 0;
                        while ((row = obj_for_reader.ReadLine()) != null)
                        {
                            single_row = row.Split(':');
                            if ((single_row[0] == "T"))
                            {
                                multiple_row = single_row[1].Split('#');
                                if (multiple_row[0] == query_word[1])
                                {
                                    single_row_no = single_row[1].ToCharArray();
                                    for (int i = 0; i < single_row[1].Length; i++)
                                    {
                                        if (single_row_no[i] == '#')
                                        {
                                            symbol_checker++;
                                        }
                                    }
                                    for (int i = 0; i < symbol_checker; i++)
                                    {
                                        if (multiple_row[i].Equals(query_word[7], StringComparison.OrdinalIgnoreCase) == true)
                                        {
                                            final_result += i + " ";
                                            check = 2;
                                        }
                                    }
                                    for (int i = 0; i < symbol_checker; i++)
                                    {
                                        if (multiple_row[i].Equals(query_word[3], StringComparison.OrdinalIgnoreCase) == true)
                                        {
                                            final_result += i;
                                            check1 = 2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if ((check == 2) && (check1 == 2))
                    {
                        int attribute_value = 1;
                        string[] attribute_no = final_result.Split(' ');
                        int where_valu = Convert.ToInt32(attribute_no[0]);
                        int set_valu = Convert.ToInt32(attribute_no[1]);
                        string row;
                        string[] single_row;
                        string[] multiple_row;
                        char[] single_row_no;
                        int symbol_checker = 0;
                        using (var obj_for_reader = new StreamReader(path))
                        {
                            using (var obj_for_writer = new StreamWriter(temporary_file, true))
                            {
                                while ((row = obj_for_reader.ReadLine()) != null)
                                {
                                    single_row = row.Split(':');
                                    if ((single_row[0] == "D"))
                                    {
                                        multiple_row = single_row[1].Split('#');
                                        if (multiple_row[0] == query_word[1])
                                        {
                                            if (multiple_row[where_valu] == query_word[9])
                                            {
                                                single_row_no = single_row[1].ToCharArray();
                                                for (int i = 0; i < single_row[1].Length; i++)
                                                {
                                                    if (single_row_no[i] == '#')
                                                    {
                                                        symbol_checker++;
                                                    }
                                                }
                                                select_from_table += "D:";
                                                for (int count = 0; count < symbol_checker; count++)
                                                {
                                                    if (count != set_valu)
                                                    {
                                                        attribute_value = 2;
                                                        select_from_table += multiple_row[count] + "#";
                                                    }
                                                    else
                                                    {
                                                        select_from_table += query_word[5] + "#";
                                                    }
                                                }
                                                symbol_checker = 0;
                                                //MessageBox.Show(select_from_table);
                                                row = select_from_table;
                                                select_from_table = "";
                                            }
                                        }
                                    }
                                    obj_for_writer.WriteLine(row);
                                }
                            }
                        }
                        if (attribute_value == 2)
                        {
                            File.Delete(path);
                            File.Move(temporary_file, path);
                            sem_rzlt.Text = "Updated successfully!!!";
                        }
                        else
                        {
                            MessageBox.Show("Wrong entry!!!");
                        }
                    }
                    else
                    {
                        sem_rzlt.Text = "Column does not exist!!!";
                    }
                }
                catch
                {
                    tab_rzlt.Text = "File does not exists!!!";
                }
            }

                /************Code for selecting a specific column*************/

            else if (cfg_result == 2)  
            {
                int count1 = 0, count2 = 0;
                string[] array_for_semantic_check = cfg.Split(' ');
                string[] finded_data;
                foreach (string s in query_word)
                {
                    if (count1.ToString() == array_for_semantic_check[count2])
                    {
                        select_from_table = select_from_table + s + "#";
                        count2++;
                    }
                    count1++;
                }
                int count_rows = 0;
                char[] array = select_from_table.ToCharArray();
                for (int counter = 0; counter < select_from_table.Length; counter++)
                {
                    if (array[counter] == '#')
                    {
                        count_rows++;
                    }
                }
                finded_data = select_from_table.Split('#');
                string temporary_file = Path.GetTempFileName();
                string row;
                string[] single_row;
                string[] multiple_row;
                char[] single_row_no;
                string array_for_index = "";
                int symbol_checker = 0;
                try
                {
                    using (var obj_for_reader = new StreamReader(path))
                    {
                        while ((row = obj_for_reader.ReadLine()) != null)
                        {
                            single_row = row.Split(':');
                            if ((single_row[0] == "T"))
                            {
                                multiple_row = single_row[1].Split('#');
                                if (finded_data[(count2 - 1)] == multiple_row[0])
                                {
                                    single_row_no = single_row[1].ToCharArray();
                                    for (int i = 0; i < single_row[1].Length; i++)
                                    {
                                        if (single_row_no[i] == '#')
                                        {
                                            symbol_checker++;
                                        }
                                    }
                                    for (int j = 1; j < symbol_checker; j++)
                                    {
                                        for (int i = 0; i < count_rows; i++)
                                        {

                                            if (finded_data[i].Equals(multiple_row[j], StringComparison.OrdinalIgnoreCase) == true)
                                            {
                                                array_for_index += j + " ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    char[] array1 = array_for_index.ToCharArray();
                    string[] array2 = array_for_index.Split(' ');
                    int space_count = 0;
                    for (int j = 0; j < array_for_index.Length; j++)
                    {
                        if (array1[j] == ' ')
                        {
                            space_count++;
                        }
                    }
                    if ((count_rows - 1) == space_count)
                    {
                        tab_rzlt.Text = "";
                        symbol_checker = 0;
                        using (var obj_for_reader = new StreamReader(path))
                        {
                            while ((row = obj_for_reader.ReadLine()) != null)
                            {
                                single_row = row.Split(':');
                                if ((single_row[0] == "D") || (single_row[0] == "T"))
                                {
                                    multiple_row = single_row[1].Split('#');
                                    if (finded_data[(count2 - 1)] == multiple_row[0])
                                    {
                                        single_row_no = single_row[1].ToCharArray();
                                        for (int i = 0; i < single_row[1].Length; i++)
                                        {
                                            if (single_row_no[i] == '#')
                                            {
                                                symbol_checker++;
                                            }
                                        }
                                        int index = 0;
                                        for (int cont = 0; cont < symbol_checker; cont++)
                                        {
                                            if (cont.ToString() == array2[index])
                                            {
                                                select_from_data += multiple_row[cont] + " ";
                                                index++;
                                            }
                                        }
                                        tab_rzlt.Text += select_from_data + "\r\n";
                                        select_from_data = "";
                                        sem_rzlt.Text = "Data retrieved successfully!!!";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tab_rzlt.Text="No data found!!!";
                        sem_rzlt.Text = "Column does not exist!!!";
                    }
                }
                catch
                {
                    tab_rzlt.Text = "File does not exist!!!";
                }
            }
        }

        private static void fa_maker_of_query(ref string tokens_of_query, string s, char[] tokens, ref int flag, int state)
        {
            int str1 = 0, str2 = 0;
            char[] states = specific_words[state].ToCharArray();
            str1 = specific_words[state].Length;
            str2 = s.Length;
            if (str1 == str2)
            {
                for (int i = 0; i < str2; i++)
                {
                    if (!tokens[i].ToString().Equals(states[i].ToString(), StringComparison.OrdinalIgnoreCase) == true)
                    {
                        flag = 0;
                    }
                }
                if (flag == 1)
                {
                    tokens_of_query += "Key-Word ";
                }
                else
                {
                    tokens_of_query += "Identifier ";
                }
            }
            else
            {
                tokens_of_query += "Identifier ";
            }
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            component_rzlt.Text = "";
            fa_rzlt.Text = "";
            input_query.Text = "";
            component_rzlt.Text = "";
            cfg_rzlt.Text = "";
            sem_rzlt.Text = "";
            tab_rzlt.Text = "";
            input_query.BackColor = System.Drawing.Color.Empty;
            fa_rzlt.BackColor = System.Drawing.Color.Empty;
            cfg_rzlt.BackColor = System.Drawing.Color.Empty;
            sem_rzlt.BackColor = System.Drawing.Color.Empty;
            tab_rzlt.BackColor = System.Drawing.Color.Empty;
            component_rzlt.BackColor = System.Drawing.Color.Empty;
           
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = System.Drawing.Color.DarkKhaki;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = System.Drawing.Color.Empty;
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = System.Drawing.Color.DarkKhaki;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = System.Drawing.Color.Empty;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
