namespace WinGPT.Taxonomy;

public partial class Taxonomy_Form : Form
{
    private const string NotApplicable = "<new category>";

    public Taxonomy_Form(Function_Parameters functionParameters)
    {
        InitializeComponent();

        this.FormClosing += Taxonomy_Form_FormClosing;

        // Bind TextBox controls to the respective properties of the function parameters
        summary_textBox.DataBindings.Add("Text", functionParameters, nameof(Function_Parameters.summary));
        filename_textBox.DataBindings.Add("Text", functionParameters, nameof(Function_Parameters.filename));
        new_category_textBox.DataBindings.Add("Text", functionParameters, nameof(Function_Parameters.new_category));

        // Create a new list for the ComboBox, including a NotApplicable option
        var categoriesWithNAOption = new List<string> { NotApplicable };
        if (functionParameters.existing_categories is null)
        {
            MessageBox.Show("No existing categories given.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            functionParameters.existing_categories = Array.Empty<string>();
        }

        categoriesWithNAOption.AddRange(functionParameters.existing_categories);

        // Set the DataSource for the ComboBox and bind its selected item to the selected category of the function parameters
        existing_categories_comboBox.DataSource = categoriesWithNAOption;
        existing_categories_comboBox.DataBindings.Add("SelectedItem", functionParameters, nameof(Function_Parameters.selected_category));

        // Prioritize the new category over the selected category
        if (!string.IsNullOrEmpty(functionParameters.new_category))
        {
            existing_categories_comboBox.SelectedItem = NotApplicable;
        }
        else if (!string.IsNullOrEmpty(functionParameters.selected_category))
        {
            existing_categories_comboBox.SelectedItem = functionParameters.selected_category;
        }
        else
        {
            existing_categories_comboBox.SelectedItem = NotApplicable;
        }

        UpdateIllegalStateLabelVisibility();

        existing_categories_comboBox.SelectedIndexChanged += (s, e) =>
        {
            if (existing_categories_comboBox.SelectedItem.ToString() != NotApplicable)
            {
                new_category_textBox.Enabled = false;
                new_category_textBox.Clear();
            }
            else
            {
                new_category_textBox.Enabled = true;
                new_category_textBox.Focus();
            }

            UpdateIllegalStateLabelVisibility();
        };

        new_category_textBox.TextChanged += (s, e) =>
        {
            if (!string.IsNullOrEmpty(new_category_textBox.Text))
            {
                existing_categories_comboBox.SelectedItem = NotApplicable;
            }

            UpdateIllegalStateLabelVisibility();
        };
    }

    private void Taxonomy_Form_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (DialogResult == DialogResult.OK)
            return;
        this.DialogResult = DialogResult.Cancel;
    }

    private void OK_button_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void reroll_button_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Retry;
        Close();
    }

    private void UpdateIllegalStateLabelVisibility()
    {
        // Show the illegal state label if both a new category and an existing category are provided,
        // or if the new category matches any existing category
        illegal_state_label.Visible =
           (!string.IsNullOrEmpty(new_category_textBox.Text) &&
            existing_categories_comboBox.SelectedItem.ToString() != NotApplicable) ||
           existing_categories_comboBox.Items.Contains(new_category_textBox.Text);
    }
}