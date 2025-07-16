<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SIDec.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class ="image-box__background op-3"></div>
	<div class="col-12 mt-3">
    <div class="row row-cols-1 row-cols-md-2 row-cols-xl-3">
      <div class="col mb-3">
        <div class="card h-100">
          <div class="card-header bg-info text-dark"><h4>Predios Declarados</h4></div>
          <div class="card-body">
            <p id="desc_predios" runat="server" class="card-text"></p>
          </div>
          <div class="card-footer bg-light t-r">
            <a href="Predios" class="text-primary"><i class="fas fa-arrow-circle-right fa-2x"></i></a>
          </div>
        </div>
      </div>
      <div class="col mb-3">
        <div class="card h-100">
          <div class="card-header bg-info text-dark"><h4>Planes Parciales</h4></div>
          <div class="card-body">
            <p id="desc_planesp" runat="server" class="card-text"></p>
          </div>
          <div class="card-footer bg-light t-r">
            <a href="PlanesP" class="text-primary"><i class="fas fa-arrow-circle-right fa-2x"></i></a>
          </div>
        </div>
      </div>
      <div class="col mb-3">
        <div class="card h-100">
          <div class="card-header bg-info text-dark"><h4>Proyectos Asociativos</h4></div>
          <div class="card-body">
            <p id="desc_proyectos" runat="server" class="card-text"></p>
          </div>
          <div class="card-footer bg-light t-r">
            <a href="Proyectos" class="text-primary"><i class="fas fa-arrow-circle-right fa-2x"></i></a>
          </div>
        </div>
      </div>
      <div class="col mb-3">
        <div class="card h-100">
          <div class="card-header bg-warning text-dark"><h4>Geosidec</h4></div>
          <div class="card-body">
            <p id="desc_geosidec" runat="server" class="card-text"></p>
          </div>
          <div class="card-footer bg-light t-r">
            <a href="http://sidec.habitatbogota.gov.co/geosidec/" target="_blank" class="text-primary"><i class="fas fa-arrow-circle-right fa-2x"></i></a>
          </div>
        </div>
      </div>
      <div class="col mb-3">
        <div class="card h-100">
          <div class="card-header bg-warning text-dark"><h4>Enlaces de interés</h4></div>
          <div class="card-body">
            <ul class="pl-0" style="list-style: none;">
              <li><i class="fas fa-angle-right text-primary mb-1"></i>&nbsp;<a href="https://observatoriohabitat.org/" target="_blank" class="text-primary">Observatorio de Hábitat</a></li>
              <li><i class="fas fa-angle-right text-primary mb-1"></i>&nbsp;<a href="https://www.vur.gov.co/siteminderagent/forms_es-ES/loginsnr.fcc?" target="_blank" class="text-primary">VUR</a></li>
              <li><i class="fas fa-angle-right text-primary mb-1"></i>&nbsp;<a href="http://vucapp.habitatbogota.gov.co/vuc/login.seam" target="_blank" class="text-primary">VUC</a></li>
              <li><i class="fas fa-angle-right text-primary mb-1"></i>&nbsp;<a href="https://sinupot.sdp.gov.co/portalp/home/" target="_blank" class="text-primary">SINUPOT</a></li>
              <li><i class="fas fa-angle-right text-primary mb-1"></i>&nbsp;<a href="https://mapas.bogota.gov.co" target="_blank" class="text-primary">IDECA - Mapa de Referencia</a></li>
            </ul>            
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>