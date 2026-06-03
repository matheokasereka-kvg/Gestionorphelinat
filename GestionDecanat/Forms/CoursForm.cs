using GestionDecanat.DAL;
namespace GestionDecanat.Forms
{
    public class CoursForm : BaseDataForm
    {
        private readonly CoursRepository repo=new CoursRepository(); private readonly FaculteRepository facRepo=new FaculteRepository(); private readonly PromotionRepository promRepo=new PromotionRepository(); private readonly System.Windows.Forms.TextBox nom,coef; private readonly System.Windows.Forms.ComboBox faculte,promotion;
        public CoursForm():base("Gestion des cours"){ Label("Cours",10,18); nom=TextBox(90,15,220); Label("Coefficient",330,18); coef=TextBox(430,15,80); Label("Faculté",530,18); faculte=Combo(610,15,200); Label("Promotion",10,58); promotion=Combo(90,55,220); BindCombo(faculte,facRepo.GetAll(),"idFaculte","nomFaculte"); BindCombo(promotion,promRepo.GetAll(),"idPromotion","nomPromotion"); }
        protected override void RefreshGrid(){grid.DataSource=repo.GetAll();} protected override void Search(){grid.DataSource=repo.Search(txtSearch.Text,"nomCours");}
        protected override void AddRecord(){repo.Insert(repo.Parameters(nom.Text,ToDecimal(coef),ToInt(faculte),ToInt(promotion)));} protected override void UpdateRecord(){repo.Update(SelectedId("idCours"),repo.Parameters(nom.Text,ToDecimal(coef),ToInt(faculte),ToInt(promotion)));} protected override void DeleteRecord(){ConfirmDelete(()=>repo.Delete(SelectedId("idCours")));}
        protected override void LoadSelected(){ if(grid.CurrentRow!=null){nom.Text=grid.CurrentRow.Cells["nomCours"].Value.ToString();coef.Text=grid.CurrentRow.Cells["coefficient"].Value.ToString();faculte.SelectedValue=grid.CurrentRow.Cells["idFaculte"].Value;promotion.SelectedValue=grid.CurrentRow.Cells["idPromotion"].Value;}}
    }
}
