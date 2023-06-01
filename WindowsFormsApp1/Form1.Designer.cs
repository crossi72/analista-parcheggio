namespace WindowsFormsApp1
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtInput = new System.Windows.Forms.TextBox();
			this.btnArrivoVeicolo = new System.Windows.Forms.Button();
			this.btnStampaVeicoli = new System.Windows.Forms.Button();
			this.btnUscitaVeicolo = new System.Windows.Forms.Button();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtInput
			// 
			this.txtInput.Location = new System.Drawing.Point(89, 89);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(291, 31);
			this.txtInput.TabIndex = 0;
			// 
			// btnArrivoVeicolo
			// 
			this.btnArrivoVeicolo.Location = new System.Drawing.Point(89, 183);
			this.btnArrivoVeicolo.Name = "btnArrivoVeicolo";
			this.btnArrivoVeicolo.Size = new System.Drawing.Size(324, 131);
			this.btnArrivoVeicolo.TabIndex = 2;
			this.btnArrivoVeicolo.Text = "Arrivo nuovo veicolo";
			this.btnArrivoVeicolo.UseVisualStyleBackColor = true;
			this.btnArrivoVeicolo.Click += new System.EventHandler(this.btnArrivoVeicolo_Click);
			// 
			// btnStampaVeicoli
			// 
			this.btnStampaVeicoli.Location = new System.Drawing.Point(450, 183);
			this.btnStampaVeicoli.Name = "btnStampaVeicoli";
			this.btnStampaVeicoli.Size = new System.Drawing.Size(324, 131);
			this.btnStampaVeicoli.TabIndex = 3;
			this.btnStampaVeicoli.Text = "Stampa veicoli nel parcheggio";
			this.btnStampaVeicoli.UseVisualStyleBackColor = true;
			this.btnStampaVeicoli.Click += new System.EventHandler(this.btnStampaVeicoli_Click);
			// 
			// btnUscitaVeicolo
			// 
			this.btnUscitaVeicolo.Location = new System.Drawing.Point(811, 183);
			this.btnUscitaVeicolo.Name = "btnUscitaVeicolo";
			this.btnUscitaVeicolo.Size = new System.Drawing.Size(324, 131);
			this.btnUscitaVeicolo.TabIndex = 4;
			this.btnUscitaVeicolo.Text = "Uscita veicolo";
			this.btnUscitaVeicolo.UseVisualStyleBackColor = true;
			this.btnUscitaVeicolo.Click += new System.EventHandler(this.btnUscitaVeicolo_Click);
			// 
			// txtOutput
			// 
			this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.txtOutput.Location = new System.Drawing.Point(89, 380);
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtOutput.Size = new System.Drawing.Size(1046, 137);
			this.txtOutput.TabIndex = 5;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1328, 549);
			this.Controls.Add(this.txtOutput);
			this.Controls.Add(this.btnUscitaVeicolo);
			this.Controls.Add(this.btnStampaVeicoli);
			this.Controls.Add(this.btnArrivoVeicolo);
			this.Controls.Add(this.txtInput);
			this.Name = "Form1";
			this.Text = "Gestione parcheggio";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Button btnArrivoVeicolo;
		private System.Windows.Forms.Button btnStampaVeicoli;
		private System.Windows.Forms.Button btnUscitaVeicolo;
		private System.Windows.Forms.TextBox txtOutput;
	}
}

