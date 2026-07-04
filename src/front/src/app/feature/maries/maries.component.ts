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
          description: "Organisée, attentive et redoutablement efficace, rien n'échappe à son radar. Capable de repérer un détail oublié à 50 mètres et un début de panique avant même qu'il ne commence.",
          mission: "Mission : maintenir le niveau de sérénité réglementaire et s'assurer que tout roule.",
          image: 'assets/pictures/témoins/astrid.png'
        },
        {
          name: 'Matthieu',
          role: 'Responsable de la protection fraternelle',
          description: "Présent depuis toujours (ou presque), il veille discrètement sur les opérations. Il parle peu, observe beaucoup et intervient uniquement lorsque la situation l'exige.",
          mission: "Mission : assurer que tout se déroule comme prévu et sortir de l'ombre en cas d'urgence.",
          image: 'assets/pictures/témoins/matthieu.jpg'
        },
        {
          name: 'Brune',
          role: 'Responsable bonne humeur & spontanéité',
          description: "Toujours partante, jamais très loin d'une bonne idée ou d'une aventure improvisée. Son énergie est communicative et elle a le don de transformer n'importe quel moment en souvenir mémorable.",
          mission: "Mission : maintenir l'ambiance à son maximum du premier verre au dernier pas de danse.",
          image: 'assets/pictures/témoins/brune.png'
        },
        {
          name: 'Stéphanie',
          role: 'Responsable des souvenirs',
          description: 'Mémoire vivante du groupe, elle conserve anecdotes, dossiers et souvenirs avec précision. Rien ne lui échappe! Son talent : rendre chaque instant un peu plus mémorable.',
          mission: 'Mission : capturer les meilleurs moments et fournir un stock inépuisable d\'anecdotes pour les années à venir.',
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
          description: "Expert en adaptation et en gestion de son temps, il sait toujours trouver une solution, une disponibilité ou un bon prétexte pour rejoindre la fête. Son seul besoin : un peu de réconfort après l'effort.",
          mission: "Mission : répondre présent au bon moment, surtout quand il faut célébrer, aider et surtout trinquer.",
          image: 'assets/pictures/témoins/thomas.jpg'
        },
        {
          name: 'Léonard',
          role: 'Responsable discipline & gestion des débordements',
          description: 'Maître incontesté du débit, il connaît parfaitement les règles, mais aussi les circonstances exceptionnelles qui permettent de les oublier temporairement.',
          mission: 'Mission : assurer que chaque verre soit rempli.',
          image: 'assets/pictures/témoins/leonard.jpg'
        },
        {
          name: 'Hugues',
          role: 'Responsable qualité',
          description: "Gardien des poules et maître de la farine, il cultive le goût des choses bien faites, que ce soit à travers la photographie ou les bons produits du quotidien. Un expert du détail en toute discrétion.",
          mission: 'Mission : garder un œil sur les détails et s\'assurer que tout se déroule dans les meilleures conditions.',
          image: 'assets/pictures/témoins/hugues.jpg'
        },
        {
          name: 'Ambroise',
          role: "Responsable de l'art de vivre",
          description: "Toujours chic et élégant, défenseur du bon goût, du bon vin et des traditions françaises. Aussi à l'aise autour d'une grande table que sur une piste de danse, il sait apprécier chaque instant comme il se doit.",
          mission: 'Mission : mettre l\'ambiance tout en savourant chaque instant.',
          image: 'assets/pictures/témoins/ambroise.jpg'
        }
      ]
    }
  ];

}
