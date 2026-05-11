import { Component } from '@angular/core';

type Witness = {
  readonly name: string;
  readonly role: string;
  readonly description: string;
  readonly mission: string;
  readonly image: string;
};

type WitnessGroup = {
  readonly key: 'bride' | 'groom';
  readonly title: string;
  readonly members: readonly Witness[];
};

@Component({
  standalone: false,
  selector: 'app-maries',
  templateUrl: './maries.component.html',
  styleUrl: './maries.component.scss'
})
export class MariesComponent {
  readonly introduction: string = "Parce qu'un mariage ne s'organise pas sans une équipe de choc... Voici notre staff officiel, soigneusement sélectionné pour nous accompagner (et nous supporter) jusqu'au jour J.";

  readonly witnessGroups: readonly WitnessGroup[] = [
    {
      key: 'bride',
      title: 'Les témoins de la mariée',
      members: [
        {
          name: 'Astrid',
          role: 'Responsable qualité & contrôle émotionnel',
          description: 'Sœur de la mariée, attentive et redoutablement efficace sur les détails.',
          mission: 'Mission : surveiller que tout soit parfait (ou presque)',
          image: 'assets/pictures/témoins/astrid.png'
        },
        {
          name: 'Mathieu',
          role: 'Responsable de la protection fraternelle',
          description: 'Frère de la mariée, présent depuis toujours (ou presque) pour veiller, discrètement... ou pas.',
          mission: 'Mission : assurer que tout roule... et intervenir si besoin.',
          image: 'assets/pictures/témoins/matthieu.jpg'
        },
        {
          name: 'Brune',
          role: 'Responsable bonne humeur & gestion des urgences',
          description: 'Énergie contagieuse et gestion des situations critiques... même à 2h du matin.',
          mission: 'Mission : accompagner la mariée du premier sourire au dernier pas de danse',
          image: 'assets/pictures/témoins/brune.png'
        },
        {
          name: 'Stéphanie',
          role: 'Responsable des souvenirs & des fous rires',
          description: 'Amie de longue date, experte en moments mémorables et discussions interminables.',
          mission: 'Mission : rappeler à la mariée de profiter (et rire encore plus)',
          image: 'assets/pictures/témoins/stéphanie.jpg'
        }
      ]
    },
    {
      key: 'groom',
      title: 'Les témoins du marié',
      members: [
        {
          name: 'Thomas',
          role: 'Responsable flexibilité & opportunités',
          description: "Expert en adaptation et en gestion de son temps, il n'exige rien d'autre qu'un peu de réconfort après l'effort. Toujours disponible pour les grandes occasions !",
          mission: "Mission : répondre présent dès qu'il faut célébrer, aider ou trinquer",
          image: 'assets/pictures/témoins/thomas.jpg'
        },
        {
          name: 'Léonard',
          role: 'Responsable discipline & gestion des débordements',
          description: 'Sait parfaitement quand faire respecter les règles... et quand les oublier.',
          mission: 'Mission : assurer que chaque verre soit rempli.',
          image: 'assets/pictures/témoins/leonard.jpg'
        },
        {
          name: 'Hugues',
          role: 'Responsable qualité terroir',
          description: "Gardien des poules et maître de la farine, c'est aussi un grand spécialiste de la Chartreuse et des bons repas. Un homme de goût... sauf, étrangement, concernant un fromage...",
          mission: 'Mission : s\'assurer que personne ne manque de pain, de vin ou de bonne humeur.',
          image: 'assets/pictures/témoins/hugues.jpg'
        },
        {
          name: 'Ambroise',
          role: "Responsable de l'art de vivre",
          description: "Toujours très chic et élégant, jamais en reste quand il s'agit de célébrer. Défenseur du bon goût, du bon vin et des traditions françaises, il savoure autant les grandes tablées que les grands moments.",
          mission: 'Mission : mettre l\'ambiance tout en savourant chaque instant',
          image: 'assets/pictures/témoins/ambroise.jpg'
        }
      ]
    }
  ];

}
